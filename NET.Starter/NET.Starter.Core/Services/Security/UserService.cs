using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NET.Starter.Core.Bases;
using NET.Starter.Core.Services.Security.Dtos;
using NET.Starter.Core.Services.Security.Inputs;
using NET.Starter.Core.Services.Security.Interfaces;
using NET.Starter.DataAccess.SqlServer;
using NET.Starter.DataAccess.SqlServer.Models.Security;
using NET.Starter.Shared.Enums;
using NET.Starter.Shared.Objects.Dtos;
using NET.Starter.Shared.Objects.Inputs;

namespace NET.Starter.Core.Services.Security
{
    internal class UserService(ApplicationDbContext dbContext, IMapper mapper, ILogger<UserService> logger)
        : BaseService<UserService>(dbContext, mapper, logger), IUserService
    {
        public async Task<BaseDto> ActivateUserAsync(Guid userId)
        {
            _logger.LogInformation("Starting to activate user by id: {UserId}.", userId);

            var user = await _dbContext.Users.FirstOrDefaultAsync(d => d.Id == userId);
            if (user == null)
            {
                _logger.LogError("User data is not found for id: {UserId}.", userId);

                return new("User data is not found", ResponseCode.NotFound);
            }

            user.IsActive = true;

            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("Successfully activated user by id: {UserId}.", userId);

            return new("User data is successfully activated", ResponseCode.Ok);
        }

        public async Task<BaseDto> CreateUserAsync(UserInput input)
        {
            _logger.LogInformation("Starting to create user.");

            var (isValid, validationMessage) = await ValidateUserInput(input);
            if (!isValid)
            {
                _logger.LogError("Failed to create user. Reason : {ValidationMessage}", validationMessage);

                return new(validationMessage, ResponseCode.Error);
            }

            var user = _mapper.Map<User>(input, opts => opts.Items["IsCreate"] = true);

            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("Successfully created user.");

            return new("User data is successfully created", ResponseCode.Ok);
        }

        public async Task<BaseDto> DeactivateUserAsync(Guid userId)
        {
            _logger.LogInformation("Starting to deactivate user by id: {UserId}.", userId);

            var user = await _dbContext.Users.FirstOrDefaultAsync(d => d.Id == userId);
            if (user == null)
            {
                _logger.LogError("User data is not found for id: {UserId}.", userId);

                return new("User data is not found", ResponseCode.NotFound);
            }

            user.IsActive = false;

            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("Successfully deactivated user by id: {UserId}.", userId);

            return new("User data is successfully deactivated", ResponseCode.Ok);
        }

        public async Task<BaseDto> DeleteUserAsync(Guid userId)
        {
            _logger.LogInformation("Starting to delete user by id: {UserId}.", userId);

            var user = await _dbContext.Users.FirstOrDefaultAsync(d => d.Id == userId);
            if (user == null)
            {
                _logger.LogError("User data is not found for id: {UserId}.", userId);

                return new("User data is not found", ResponseCode.NotFound);
            }

            user.RowStatus = 1;

            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("Successfully deleted user by id: {UserId}.", userId);

            return new("User data is successfully deleted", ResponseCode.Ok);
        }

        public async Task<ObjectDto<UserDto>> RetrieveUserByIdAsync(Guid userId)
        {
            _logger.LogInformation("Starting to retrieve user by id: {UserId}.", userId);

            var user = await _dbContext.Users.Include(u => u.UserRoles).ThenInclude(ur => ur.Role).AsNoTracking().FirstOrDefaultAsync(d => d.Id == userId);
            if (user == null)
            {
                _logger.LogError("User data is not found for id: {UserId}.", userId);

                return new("User data is not found", ResponseCode.NotFound);
            }

            _logger.LogInformation("Successfully retrieved user by id: {UserId}.", userId);

            return new(responseCode: ResponseCode.Ok)
            {
                Obj = _mapper.Map<UserDto>(user)
            };
        }

        public PagingDto<UserDto> RetrieveUsersPaging(PagingSearchInputBase input)
        {
            _logger.LogInformation("Starting to retrieve paging of users.");

            var retVal = new PagingDto<UserDto>();

            var searchKey = input.SearchKey?.Trim() ?? string.Empty;
            var searchPattern = $"%{searchKey}%";

            var users = _dbContext.Users.AsNoTracking()
                                        .Where(d => 
                                            EF.Functions.Like(d.Username, searchPattern) ||
                                            EF.Functions.Like(d.Fullname, searchPattern) || 
                                            EF.Functions.Like(d.EmailAddress, searchPattern)
                                        )
                                        .OrderByDescending(d => d.Modified ?? d.Created)
                                        .Select(d => _mapper.Map<UserDto>(d));

            retVal.ApplyPagination(input.Page, input.PageSize, users);

            _logger.LogInformation("Successfully retrieved paging of users.");

            return retVal;
        }

        public async Task<BaseDto> UpdateUserAsync(Guid userId, UserInput input)
        {
            _logger.LogInformation("Starting to update user by id: {UserId}.", userId);

            var (isValid, validationMessage) = await ValidateUserInput(input, userId);
            if (!isValid)
            {
                _logger.LogError("Failed to update user. Reason : {ValidationMessage}", validationMessage);

                return new(validationMessage, ResponseCode.Error);
            }

            var user = await _dbContext.Users.Include(u => u.UserRoles).FirstOrDefaultAsync(d => d.Id == userId);
            if (user == null)
            {
                _logger.LogError("User data is not found for id: {UserId}.", userId);

                return new("User data is not found", ResponseCode.NotFound);
            }

            _mapper.Map(input, user);

            #region Delete role does not exists on input

            var deleteRoles = from d in user.UserRoles
                              where !input.RoleIds.Contains(d.RoleId)
                              select d;

            foreach (var userRole in deleteRoles)
            {
                userRole.RowStatus = 1;
            }

            #endregion

            #region Add role does not exists on input

            var addRoles = from i in input.RoleIds
                           where !user.UserRoles.Any(d => d.RoleId == i)
                           select i;

            if (addRoles.Any())
                await _dbContext.UserRoles.AddRangeAsync(addRoles.Select(d => new UserRole { UserId = user.Id, RoleId = d }));

            #endregion

            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("Successfully updated user by id: {UserId}.", userId);

            return new("User data is successfully updated", ResponseCode.Ok);
        }

        /// <summary>
        /// Validates whether the provided password meets the security requirements.
        /// </summary>
        /// <param name="password">The password string to be validated. It should meet the defined security criteria such as length, special characters, or case sensitivity.</param>
        /// <returns>
        /// A tuple containing:
        /// <list type="bullet">
        ///   <item><c>bool</c>: <c>true</c> if the password meets all security requirements; otherwise, <c>false</c>.</item>
        ///   <item><c>string</c>: A message describing the validation result (e.g., "Password is too short", "Password must contain a special character").</item>
        /// </list>
        /// </returns>
        private static (bool isValid, string message) PasswordValidation(string password)
        {
            if (password.Length < 10)
            {
                return (false, "Password must be at least 10 characters long");
            }

            if (!password.Any(char.IsUpper))
            {
                return (false, "Password must contain at least one uppercase letter");
            }

            if (!password.Any(char.IsLower))
            {
                return (false, "Password must contain at least one lowercase letter");
            }

            if (!password.Any(char.IsPunctuation))
            {
                return (false, "Password must contain at least one special character");
            }

            if (!password.Any(char.IsDigit))
            {
                return (false, "Password must contain at least one digit");
            }

            return (true, string.Empty);
        }

        private async Task<(bool isValid, string validationMessage)> ValidateUserInput(UserInput input, Guid? userId = null)
        {
            var (isValidPassword, messageValidPassword) = PasswordValidation(input.Password);
            if (!isValidPassword)
                return (false, messageValidPassword);

            if (!input.RoleIds.Any())
                return (false, "Please add at least one role.");

            var dataDuplicateUser = await _dbContext.Users.FirstOrDefaultAsync(d => 
                (d.Username == input.Username || d.EmailAddress == input.EmailAddress) &&
                d.Id != userId
            );
            if (dataDuplicateUser != null)
                return (false, "Username or email address already exists.");

            return (true, string.Empty);
        }
    }
}
