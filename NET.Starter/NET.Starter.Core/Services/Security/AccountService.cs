using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NET.Starter.Core.Bases;
using NET.Starter.Core.Services.Organization.Dtos;
using NET.Starter.Core.Services.Security.CustomModels;
using NET.Starter.Core.Services.Security.Inputs;
using NET.Starter.Core.Services.Security.Interfaces;
using NET.Starter.DataAccess.SqlServer;
using NET.Starter.DataAccess.SqlServer.Models.Security;
using NET.Starter.Shared.Enums;
using NET.Starter.Shared.Helpers;
using NET.Starter.Shared.Objects;
using NET.Starter.Shared.Objects.Configs;
using NET.Starter.Shared.Objects.Dtos;

namespace NET.Starter.Core.Services.Security
{
    internal class AccountService(
        ApplicationDbContext dbContext, 
        IMapper mapper, 
        ILogger<AccountService> logger,
        IOptions<SecurityConfig> securityConfig, 
        CurrentUserAccessor currentUserAccessor,
        TokenService tokenService) : BaseService<AccountService>(dbContext, mapper, logger), IAccountService
    {
        private readonly SecurityConfig _securityConfig = securityConfig.Value;
        private readonly CurrentUserAccessor _currentUserAccessor = currentUserAccessor;

        private readonly TokenService _tokenService = tokenService;

        public async Task<ObjectDto<TokenResult>> LoginAsync(LoginInput input)
        {
            _logger.LogInformation("Initiating login for user: {UserIdentifier}.", input.UserIdentifier);

            var user = await _dbContext.Users.Include(u => u.UserCompanies)
                                                .ThenInclude(u => u.UserCompanyRoles)
                                                    .ThenInclude(ur => ur.Role)
                                                        .ThenInclude(r => r.RolePermissions)
                                                            .ThenInclude(rp => rp.Permission)
                                             .FirstOrDefaultAsync(d =>
                                                EF.Functions.Like(d.Username, $"{input.UserIdentifier}") || 
                                                EF.Functions.Like(d.EmailAddress, $"{input.UserIdentifier}")
                                             );

            if (user == null)
            {
                _logger.LogError("Login failed for user: {UserIdentifier}. Reason: {ErrorMessage}.", input.UserIdentifier, "User not found");

                return new("The username or password you entered is incorrect.", ResponseCode.UnAuthorized);
            }

            if (CryptographyHelper.VerifyPassword(input.Password, user.Password) == PasswordVerificationResult.Failed)
            {
                _logger.LogError("Login failed for user: {UserIdentifier}. Reason: {ErrorMessage}.", input.UserIdentifier, "Incorrect password");

                await HandleBadPasswordAttemptAsync(input.UserIdentifier, user);

                return new("The username or password you entered is incorrect.", ResponseCode.UnAuthorized);
            }

            if (user.LockedUntil >= DateTime.UtcNow)
            {
                var lockedUntilSystemTimeZone = TimeZoneHelper.ConvertToTimezoneId(user.LockedUntil.Value);
                var errorMessage = $"Account is locked until {lockedUntilSystemTimeZone:dd-MM-yyyy HH:mm:ss}";

                _logger.LogWarning("Login failed for user: {UserIdentifier}. Reason: {ErrorMessage}.", input.UserIdentifier, errorMessage);

                return new($"Your account is locked until {lockedUntilSystemTimeZone:dd-MM-yyyy HH:mm:ss}, please try again later.", ResponseCode.Forbidden);
            }

            // Reset bad password count and locked until when login is successful
            user.BadPasswordCount = 0;
            user.LockedUntil = null;

            await _dbContext.SaveChangesAsync();

            var tokenResult = ProcessGenerateToken(user);

            _logger.LogInformation("Login successful for user: {UserIdentifier}.", input.UserIdentifier);

            return new(responseCode: ResponseCode.Ok) 
            { 
                Obj = tokenResult 
            };
        }

        public async Task<ObjectDto<TokenResult>> RefreshTokenAsync(Guid? companyId = null)
        {
            companyId ??= _currentUserAccessor.CompanyId;

            _logger.LogInformation("Generate token for user Id: {UserId} to companyId: {CompanyId}.", _currentUserAccessor.UserId, companyId);

            var user = await _dbContext.Users.Include(u => u.UserCompanies)
                                                .ThenInclude(u => u.UserCompanyRoles)
                                                    .ThenInclude(ur => ur.Role)
                                                        .ThenInclude(r => r.RolePermissions)
                                                            .ThenInclude(rp => rp.Permission)
                                             .FirstOrDefaultAsync(d => d.Id == _currentUserAccessor.UserId);

            if (user == null)
            {
                _logger.LogError("Generate token failed for user Id: {UserId}. Reason: {ErrorMessage}.", _currentUserAccessor.UserId, "User not found");

                return new("Generate token failed because user not found.", ResponseCode.NotFound);
            }

            var tokenResult = ProcessGenerateToken(user, companyId);

            _logger.LogInformation("Successfully generated token for user Id: {UserId} to companyId: {CompanyId}.", _currentUserAccessor.UserId, companyId);

            return new(responseCode: ResponseCode.Ok)
            {
                Obj = tokenResult
            };
        }

        public async Task<ObjectDto<IEnumerable<CompanyDto>>> RetrieveMyCompaniesAsync()
        {
            _logger.LogInformation("Retrieving companies for user Id: {UserId}.", _currentUserAccessor.UserId);

            var myCompanies = await _dbContext.UserCompanies.Include(d => d.Company).Where(d => d.UserId == _currentUserAccessor.UserId).ToListAsync();
            if (myCompanies == null || myCompanies.Count == 0)
            {
                _logger.LogWarning("No companies found for user Id: {UserId}.", _currentUserAccessor.UserId);

                return new("No companies are assigned to your account.", ResponseCode.NotFound);
            }

            _logger.LogInformation("Successfully retrieved companies for user ID: {UserId}.", _currentUserAccessor.UserId);

            return new(responseCode: ResponseCode.Ok)
            {
                Obj = myCompanies.Select(d => _mapper.Map<CompanyDto>(d.Company))
            };
        }

        /// <summary>
        /// Handles the process of recording a bad password attempt for a user.
        /// </summary>
        /// <param name="userIdentifier">The identifier of the user.</param>
        /// <param name="user">The user entity to update.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        private async Task HandleBadPasswordAttemptAsync(string userIdentifier, User user)
        {
            user.BadPasswordCount += 1;

            if (user.BadPasswordCount == _securityConfig.MaxLoginRetry)
            {
                user.BadPasswordCount = 0;
                user.LockedUntil = DateTime.UtcNow.AddMinutes(_securityConfig.AutoUnlockAfter);

                var lockedUntilSystemTimeZone = TimeZoneHelper.ConvertToTimezoneId(user.LockedUntil.Value);

                _logger.LogInformation("User {UserIdentifier} has exceeded the maximum number of failed login attempts and is now locked until {LockedUntil}.",
                    userIdentifier,
                    lockedUntilSystemTimeZone.ToString("dd-MM-yyyy HH:mm:ss"));
            }

            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Processes token generation for the specified user, based on the selected or default company.
        /// </summary>
        /// <param name="user">The user for whom the token is generated.</param>
        /// <param name="companyId">
        /// The ID of the company that selected by the user. 
        /// If there is no selected company, the default company or randomly selected if there is no default company will be used.
        /// </param>
        /// <returns>A <see cref="TokenResult"/> containing the generated token and related information.</returns>
        /// <exception cref="UnauthorizedAccessException">
        /// Thrown when the selected company is not found or the user is not assigned to any company.
        /// </exception>
        private TokenResult ProcessGenerateToken(User user, Guid? companyId = null)
        {
            UserCompany? userCompany;

            if (companyId.HasValue)
            {
                userCompany = user.UserCompanies.FirstOrDefault(d => d.CompanyId == companyId.Value);
            }
            else
            {
                userCompany = user.UserCompanies.FirstOrDefault(d => d.IsDefault) ?? user.UserCompanies.FirstOrDefault();
            }

            if (userCompany == null)
            {
                _logger.LogWarning("Generate token failed, because user: {UserId} doesn't have any company assigned / selected company.", user.Id);

                throw new UnauthorizedAccessException("Your account is not authorized to access.");
            }
            
            var permissions = userCompany.UserCompanyRoles.SelectMany(ur => ur.Role.RolePermissions).Select(rp => rp.Permission.PermissionCode).Distinct();
            var tokenResult = _tokenService.GenerateToken(user, userCompany.CompanyId, permissions);

            return tokenResult;
        }
    }
}
