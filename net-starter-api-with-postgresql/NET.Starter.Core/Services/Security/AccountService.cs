using AutoMapper;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NET.Starter.Core.Services.Organization.Dtos;
using NET.Starter.Core.Services.Security.CustomModels;
using NET.Starter.Core.Services.Security.Dtos;
using NET.Starter.Core.Services.Security.Inputs;
using NET.Starter.Core.Services.Security.Interfaces;
using NET.Starter.DataAccess;
using NET.Starter.DataAccess.Models.Security;
using NET.Starter.Shared.Helpers;
using NET.Starter.Shared.Objects;
using NET.Starter.Shared.Objects.Configs;
using NET.Starter.Shared.Objects.Dtos;
using System.Net;
using static Google.Apis.Auth.GoogleJsonWebSignature;

namespace NET.Starter.Core.Services.Security
{
    internal class AccountService(
        ApplicationDbContext _dbContext
        , IMapper _mapper
        , ILogger<AccountService> _logger
        , IOptions<AuthenticationConfig> _securityConfig
        , CurrentUserAccessor _currentUserAccessor
        , TokenService _tokenService) : IAccountService
    {
        public async Task<ObjectDto<LoginDto>> LoginAsync(LoginInput input)
        {
            _logger.LogInformation("Initiating login for user: {UserIdentifier}.", input.UserIdentifier);

            var user = await _dbContext.Users.Include(u => u.UserCompanies)
                                                .ThenInclude(uc => uc.UserCompanyRoles)
                                                    .ThenInclude(ucr => ucr.Role)
                                                        .ThenInclude(r => r.RolePermissions)
                                                            .ThenInclude(rp => rp.Permission)
                                             .FirstOrDefaultAsync(d =>
                                                EF.Functions.Like(d.Username, $"{input.UserIdentifier}") ||
                                                EF.Functions.ILike(d.EmailAddress, $"{input.UserIdentifier}")
                                             );

            if (user is null)
            {
                _logger.LogError("Login failed for user: {UserIdentifier}. Reason: User not found.", input.UserIdentifier);

                return new("The username or password you entered is incorrect.", HttpStatusCode.Unauthorized);
            }

            if (CryptographyHelper.VerifyPassword(input.Password, user.Password) == PasswordVerificationResult.Failed)
            {
                _logger.LogError("Login failed for user: {UserIdentifier}. Reason: Incorrect password.", input.UserIdentifier);

                await HandleBadPasswordAttemptAsync(input.UserIdentifier, user);

                return new("The username or password you entered is incorrect.", HttpStatusCode.Unauthorized);
            }

            if (user.LockedUntil >= DateTime.UtcNow)
            {
                var lockedUntilSystemTimeZone = TimeZoneHelper.ConvertToTimezoneId(user.LockedUntil.Value);
                var errorMessage = $"Account is locked until {lockedUntilSystemTimeZone:dd-MM-yyyy HH:mm:ss}";

                _logger.LogWarning("Login failed for user: {UserIdentifier}. Reason: {ErrorMessage}.", input.UserIdentifier, errorMessage);

                return new($"Your account is locked until {lockedUntilSystemTimeZone:dd-MM-yyyy HH:mm:ss}, please try again later.", HttpStatusCode.Forbidden);
            }

            // Reset failed login count on successful login
            user.BadPasswordCount = 0;
            user.LockedUntil = null;

            await _dbContext.SaveChangesAsync();

            var tokenResult = ProcessGenerateToken(user);
            var loginResult = _mapper.Map<LoginDto>(tokenResult, opts => {
                opts.Items["FullName"] = user.FullName;
                opts.Items["PictureUrl"] = user.PictureUrl;
            });

            _logger.LogInformation("Login successful for user: {UserIdentifier}.", input.UserIdentifier);

            return new(httpStatusCode: HttpStatusCode.OK)
            {
                Obj = loginResult
            };
        }

        public async Task<ObjectDto<LoginDto>> GoogleLoginAsync(GoogleLoginInput input)
        {
            _logger.LogInformation("Initiating Google login with id token: {IdToken}.", input.IdToken);

            Payload payload = new();
            try
            {
                payload = await ValidateAsync(input.IdToken, new ValidationSettings
                {
                    Audience = [_securityConfig.Value.OAuthProvider.Google.ClientId]
                });
            }
            catch (InvalidJwtException ex)
            {
                _logger.LogWarning(ex, "Google ID token validation failed.");
                return new("Failed to validate your Google account.", HttpStatusCode.Unauthorized);
            }

            var user = await _dbContext.Users.Include(u => u.UserCompanies)
                                                .ThenInclude(uc => uc.UserCompanyRoles)
                                                    .ThenInclude(ucr => ucr.Role)
                                                        .ThenInclude(r => r.RolePermissions)
                                                            .ThenInclude(rp => rp.Permission)
                                             .FirstOrDefaultAsync(d => EF.Functions.ILike(d.EmailAddress, $"{payload.Email}"));

            if (user is null)
            {
                _logger.LogError("Login with Google OAuth failed. Reason: User not found.");
                return new("You are not registered with us. Please contact your administrator.", HttpStatusCode.Unauthorized);
            }

            if (!user.IsActive)
            {
                _logger.LogError("Login with Google OAuth failed. Reason: User is inactive.");
                return new("Your account is not active. Please contact your administrator.", HttpStatusCode.Unauthorized);
            }

            if (string.IsNullOrWhiteSpace(user.PictureUrl))
            {
                // Update user's picture URL from Google payload if missing
                user.PictureUrl = payload.Picture;
                await _dbContext.SaveChangesAsync();
            }

            var tokenResult = ProcessGenerateToken(user);
            var loginResult = _mapper.Map<LoginDto>(tokenResult, opts => {
                opts.Items["FullName"] = user.FullName;
                opts.Items["PictureUrl"] = user.PictureUrl;
            });

            _logger.LogInformation("Login with Google OAuth successful.");

            return new(httpStatusCode: HttpStatusCode.OK)
            {
                Obj = loginResult
            };
        }

        public async Task<ObjectDto<LoginDto>> RefreshTokenAsync(Guid? companyId = null)
        {
            companyId ??= _currentUserAccessor.CompanyId;

            _logger.LogInformation("Generate token for user Id: {UserId} to companyId: {CompanyId}.", _currentUserAccessor.UserId, companyId);

            var user = await _dbContext.Users.Include(u => u.UserCompanies)
                                                .ThenInclude(uc => uc.UserCompanyRoles)
                                                    .ThenInclude(ucr => ucr.Role)
                                                        .ThenInclude(r => r.RolePermissions)
                                                            .ThenInclude(rp => rp.Permission)
                                             .AsNoTracking()
                                             .FirstOrDefaultAsync(d => d.Id == _currentUserAccessor.UserId);

            if (user is null)
            {
                _logger.LogError("Generate token failed for user Id: {UserId}. Reason: User not found.", _currentUserAccessor.UserId);
                return new("Generate token failed because user not found.", HttpStatusCode.NotFound);
            }

            var tokenResult = ProcessGenerateToken(user, companyId);
            var loginResult = _mapper.Map<LoginDto>(tokenResult, opts => {
                opts.Items["FullName"] = user.FullName;
                opts.Items["PictureUrl"] = user.PictureUrl;
            });

            _logger.LogInformation("Successfully generated token for user Id: {UserId} to companyId: {CompanyId}.", _currentUserAccessor.UserId, companyId);

            return new(httpStatusCode: HttpStatusCode.OK)
            {
                Obj = loginResult
            };
        }

        public async Task<ObjectDto<IEnumerable<CompanyDto>>> RetrieveMyCompaniesAsync()
        {
            _logger.LogInformation("Retrieving companies for user Id: {UserId}.", _currentUserAccessor.UserId);

            var myCompanies = await _dbContext.Users.AsNoTracking()
                                                    .Where(d => d.Id == _currentUserAccessor.UserId)
                                                    .SelectMany(d => d.UserCompanies.Select(uc => uc.Company))
                                                    .ToListAsync();

            if (myCompanies is null || myCompanies.Count == 0)
            {
                _logger.LogWarning("No companies found for user Id: {UserId}.", _currentUserAccessor.UserId);
                return new("No companies are assigned to your account.", HttpStatusCode.NotFound);
            }

            _logger.LogInformation("Successfully retrieved companies for user ID: {UserId}.", _currentUserAccessor.UserId);

            return new(httpStatusCode: HttpStatusCode.OK)
            {
                Obj = myCompanies.Select(_mapper.Map<CompanyDto>)
            };
        }

        private async Task HandleBadPasswordAttemptAsync(string userIdentifier, User user)
        {
            user.BadPasswordCount++;

            if (user.BadPasswordCount == _securityConfig.Value.LoginPolicy.MaxLoginRetry)
            {
                user.BadPasswordCount = 0;
                user.LockedUntil = DateTime.UtcNow.AddMinutes(_securityConfig.Value.LoginPolicy.AutoUnlockAfter);

                var lockedUntilSystemTimeZone = TimeZoneHelper.ConvertToTimezoneId(user.LockedUntil.Value);

                _logger.LogInformation("User {UserIdentifier} locked out until {LockedUntil} due to failed login attempts.",
                    userIdentifier,
                    lockedUntilSystemTimeZone.ToString("dd-MM-yyyy HH:mm:ss"));
            }

            await _dbContext.SaveChangesAsync();
        }

        private TokenResult ProcessGenerateToken(User user, Guid? companyId = null)
        {
            var userCompany = companyId.HasValue 
                ? user.UserCompanies.FirstOrDefault(d => d.CompanyId == companyId.Value)
                : user.UserCompanies.FirstOrDefault(d => d.IsDefault) ?? user.UserCompanies.FirstOrDefault();

            if (userCompany is null)
            {
                _logger.LogWarning("Token generation failed: User {UserId} has no assigned or selected company.", user.Id);
                throw new UnauthorizedAccessException("Your account is not authorized to access.");
            }

            var permissions = userCompany.UserCompanyRoles.SelectMany(ur => ur.Role.RolePermissions).Select(rp => rp.Permission.Code).Distinct();
            var tokenResult = _tokenService.GenerateToken(user, userCompany.CompanyId, permissions);

            return tokenResult;
        }
    }
}
