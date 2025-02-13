using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NET.Starter.Core.Bases;
using NET.Starter.Core.Services.Security.Dtos;
using NET.Starter.Core.Services.Security.Inputs;
using NET.Starter.Core.Services.Security.Interfaces;
using NET.Starter.DataAccess.SqlServer;
using NET.Starter.Shared.Constants;
using NET.Starter.Shared.Enums;
using NET.Starter.Shared.Objects.Dtos;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace NET.Starter.Core.Services.Security
{
    /// <summary>
    /// Provides methods for managing user account-related operations, including authentication.
    /// </summary>
    /// <param name="dbContext">The database context used for database operations.</param>
    /// <param name="mapper">The mapper service for object mapping.</param>
    /// <param name="logger">The logger service for capturing logs specific to the derived service.</param>
    /// <param name="tokenService">The token service for generating and validating tokens.</param>
    internal class AccountService(ApplicationDbContext dbContext, IMapper mapper, ILogger<AccountService> logger, TokenService tokenService)
        : BaseService<AccountService>(dbContext, mapper, logger), IAccountService
    {
        private readonly TokenService _tokenService = tokenService;

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
                _logger.LogWarning("Login attempt failed for user identifier: {UserIdentifier}.", input.UserIdentifier);

                await HandleBadPasswordAttemptAsync(input.UserIdentifier);

                return new("There is something wrong with your username or password.", ResponseCode.UnAuthorized);
            }

            // Reset bad password count upon successful login
            if (user.BadPasswordCount > 0)
            {
                user.BadPasswordCount = 0;
                await _dbContext.SaveChangesAsync();
            }

            var permissions = user.UserRoles.SelectMany(ur => ur.Role.RolePermissions).Select(rp => rp.Permission.PermissionCode).Distinct();
            var tokenResult = _tokenService.GenerateToken(user, permissions);

            // Map user and token data to LoginDto
            var loginDto = _mapper.Map<LoginDto>(user, opts => opts.Items["MapSpecificProperties"] = true);
            _mapper.Map(tokenResult, loginDto);

            _logger.LogInformation("Login process successfully completed for user identifier: {UserIdentifier}.", input.UserIdentifier);

            return new ObjectDto<LoginDto>("Login successful.", ResponseCode.Ok)
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

            user.BadPasswordCount += 1;

            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("User {UserIdentifier} has entered a bad password. Attempt count: {BadPasswordCount}", userIdentifier, user.BadPasswordCount);
        }
    }
}
