using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NET.Starter.Core.Bases;
using NET.Starter.Core.Services.Security.Dtos;
using NET.Starter.Core.Services.Security.Inputs;
using NET.Starter.Core.Services.Security.Interfaces;
using NET.Starter.DataAccess.SqlServer;
using NET.Starter.Shared.Constants;
using NET.Starter.Shared.Enums;
using NET.Starter.Shared.Objects.Configs;
using NET.Starter.Shared.Objects.Dtos;

namespace NET.Starter.Core.Services.Security
{
    /// <summary>
    /// Provides methods for managing user account-related operations, including authentication.
    /// </summary>
    /// <param name="dbContext">The database context used for database operations.</param>
    /// <param name="mapper">The mapper service for object mapping.</param>
    /// <param name="logger">The logger service for capturing logs specific to the derived service.</param>
    /// <param name="tokenService">The token service for generating and validating tokens.</param>
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
                                                (
                                                    EF.Functions.Like(d.Username, $"{input.UserIdentifier}") || 
                                                    EF.Functions.Like(d.EmailAddress, $"{input.UserIdentifier}")
                                                ) &&
                                                EF.Functions.Collate(d.Password, CollationConstants.SQL_Latin1_General_CP1_CS_AS) == input.Password
                                             );

            if (user == null)
            {
                _logger.LogError("Login attempt failed for user identifier: {UserIdentifier}.", input.UserIdentifier);

                await HandleBadPasswordAttemptAsync(input.UserIdentifier);

                return new("There is something wrong with your username or password.", ResponseCode.UnAuthorized);
            }

            if (user.LockedUntil >= DateTime.UtcNow)
            {
                _logger.LogWarning("Login attempt failed for user identifier: {UserIdentifier}. Account is locked until: {LockedUntil}.", input.UserIdentifier, user.LockedUntil.Value.ToString("dd-MM-yyyy HH:mm:ss+00:00"));

                return new("Your account is locked", ResponseCode.Forbidden);
            }

            user.BadPasswordCount = 0;
            user.LockedUntil = null;

            await _dbContext.SaveChangesAsync();

            var permissions = user.UserRoles.SelectMany(ur => ur.Role.RolePermissions).Select(rp => rp.Permission.PermissionCode).Distinct();
            var tokenResult = _tokenService.GenerateToken(user, permissions);

            // Map user and token data to LoginDto
            var loginDto = _mapper.Map<LoginDto>(user, opts => opts.Items["MapSpecificProperties"] = true);
            _mapper.Map(tokenResult, loginDto);

            _logger.LogInformation("Login attempt successfully for user identifier: {UserIdentifier}.", input.UserIdentifier);

            return new("Login successful.", ResponseCode.Ok)
            {
                Obj = loginDto
            };
        }

        /// <summary>
        /// Handles the process of recording a bad password attempt for a user.
        /// </summary>
        /// <param name="input">The login input containing the user identifier (username or email) and password.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <remarks>
        /// If the user is found in the database, their bad password count is incremented and saved.
        /// Logs the bad password attempt along with the updated count.
        /// </remarks>
        private async Task HandleBadPasswordAttemptAsync(string userIdentifier)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(d => d.Username == userIdentifier || d.EmailAddress == userIdentifier);
            if (user == null)
                return;

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
