using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NET.Starter.Core.Bases;
using NET.Starter.Core.Services.Security.Dtos;
using NET.Starter.Core.Services.Security.Inputs;
using NET.Starter.Core.Services.Security.Interfaces;
using NET.Starter.DataAccess.SqlServer;
using NET.Starter.DataAccess.SqlServer.Models.Security;
using NET.Starter.Shared.Enums;
using NET.Starter.Shared.Helpers;
using NET.Starter.Shared.Objects.Configs;
using NET.Starter.Shared.Objects.Dtos;

namespace NET.Starter.Core.Services.Security
{
    internal class AccountService(
        ApplicationDbContext dbContext, 
        IMapper mapper, 
        ILogger<AccountService> logger, 
        TokenService tokenService, 
        IOptions<SecurityConfig> securityConfig) : BaseService<AccountService>(dbContext, mapper, logger), IAccountService
    {
        private readonly TokenService _tokenService = tokenService;
        private readonly SecurityConfig _securityConfig = securityConfig.Value;

        public async Task<ObjectDto<LoginDto>> LoginAsync(LoginInput input)
        {
            _logger.LogInformation("Starting login process for user identifier: {UserIdentifier}.", input.UserIdentifier);

            var user = await _dbContext.Users.Include(u => u.UserRoles)
                                                .ThenInclude(ur => ur.Role)
                                                    .ThenInclude(r => r.RolePermissions)
                                                        .ThenInclude(rp => rp.Permission)
                                             .FirstOrDefaultAsync(d =>
                                                EF.Functions.Like(d.Username, $"{input.UserIdentifier}") || 
                                                EF.Functions.Like(d.EmailAddress, $"{input.UserIdentifier}")
                                             );

            // Check if user exists
            if (user == null)
            {
                _logger.LogError("Login attempt failed for user identifier: {UserIdentifier}, {ErrorMessage}.", input.UserIdentifier, "User not found");

                return new("There is something wrong with your username or password.", ResponseCode.UnAuthorized);
            }

            // Check if password is correct
            if (CryptographyHelper.VerifyPassword(input.Password, user.Password) == PasswordVerificationResult.Failed)
            {
                _logger.LogError("Login attempt failed for user identifier: {UserIdentifier}, {ErrorMessage}.", input.UserIdentifier, "Wrong password.");

                await HandleBadPasswordAttemptAsync(input.UserIdentifier, user);

                return new("There is something wrong with your username or password.", ResponseCode.UnAuthorized);
            }

            if (user.LockedUntil >= DateTime.UtcNow)
            {
                var errorMessage = $"Account is locked until: {user.LockedUntil.Value:dd-MM-yyyy HH:mm:ss+00:00}.";
                _logger.LogWarning("Login attempt failed for user identifier: {UserIdentifier}. {ErrorMessage}.", input.UserIdentifier, errorMessage);

                return new("Your account is locked, please try again later.", ResponseCode.Forbidden);
            }

            // Reset bad password count and locked until when login is successful
            user.BadPasswordCount = 0;
            user.LockedUntil = null;

            await _dbContext.SaveChangesAsync();

            var permissions = user.UserRoles.SelectMany(ur => ur.Role.RolePermissions).Select(rp => rp.Permission.PermissionCode).Distinct();
            var tokenResult = _tokenService.GenerateToken(user, permissions);

            var loginDto = _mapper.Map<LoginDto>(user);
            _mapper.Map(tokenResult, loginDto);

            _logger.LogInformation("Login attempt successfully for user identifier: {UserIdentifier}.", input.UserIdentifier);

            return new(responseCode: ResponseCode.Ok)
            {
                Obj = loginDto
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
            if (user.LockedUntil >= DateTime.UtcNow)
            {
                user.BadPasswordCount += 1;
                user.LockedUntil = null;

                _logger.LogInformation("User {UserIdentifier} has entered a bad password. Attempt count: {BadPasswordCount}", userIdentifier, user.BadPasswordCount);
            }

            if (user.BadPasswordCount >= _securityConfig.MaxLoginRetry)
            {
                user.BadPasswordCount = 0;
                user.LockedUntil = DateTime.UtcNow.AddMinutes(_securityConfig.AutoUnlockAfter);

                _logger.LogInformation("User {UserIdentifier} has entered a bad password too many times. Locked until: {LockedUntil}",
                    userIdentifier,
                    user.LockedUntil.Value.ToString("dd-MM-yyyy HH:mm:ss+00:00"));
            }

            await _dbContext.SaveChangesAsync();
        }
    }
}
