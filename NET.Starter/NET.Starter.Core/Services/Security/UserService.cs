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
            _logger.LogInformation("Starting user activation for Id: {UserId}.", userId);

            var user = await _dbContext.Users.FirstOrDefaultAsync(d => d.Id == userId);
            if (user == null)
            {
                _logger.LogError("User not found for Id: {UserId}.", userId);

                return new("User not found.", ResponseCode.NotFound);
            }

            user.IsActive = true;

            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("User activation successful for Id: {UserId}.", userId);

            return new("User has been successfully activated.", ResponseCode.Ok);
        }

        public async Task<BaseDto> CreateUserAsync(UserInput input)
        {
            _logger.LogInformation("Starting user creation: {Username}.", input.Username);

            var (isValid, validationMessage) = await ValidateUserInput(input);
            if (!isValid)
            {
                _logger.LogError("User creation failed. Reason : {ErrorMessage}.", validationMessage);

                return new(validationMessage, ResponseCode.Error);
            }

            var user = _mapper.Map<User>(input, opts => opts.Items["IsCreate"] = true);

            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("User creation successful: {Username}.", user.Username);

            return new("User has been successfully created.", ResponseCode.Ok);
        }

        public async Task<BaseDto> DeactivateUserAsync(Guid userId)
        {
            _logger.LogInformation("Starting user deactivation for Id: {UserId}.", userId);

            var user = await _dbContext.Users.FirstOrDefaultAsync(d => d.Id == userId);
            if (user == null)
            {
                _logger.LogError("User not found for Id: {UserId}.", userId);

                return new("User not found.", ResponseCode.NotFound);
            }

            user.IsActive = false;

            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("User deactivation successful for Id: {UserId}.", userId);

            return new("User has been successfully deactivated.", ResponseCode.Ok);
        }

        public async Task<BaseDto> DeleteUserAsync(Guid userId)
        {
            _logger.LogInformation("Starting user deletion for Id: {UserId}.", userId);

            var user = await _dbContext.Users.FirstOrDefaultAsync(d => d.Id == userId);
            if (user == null)
            {
                _logger.LogError("User not found for Id: {UserId}.", userId);

                return new("User not found.", ResponseCode.NotFound);
            }

            user.RowStatus = 1;

            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("User deletion successful for Id: {UserId}.", userId);

            return new("User has been successfully deleted.", ResponseCode.Ok);
        }

        public async Task<ObjectDto<UserDto>> RetrieveUserByIdAsync(Guid userId)
        {
            _logger.LogInformation("Starting to retrieve user for Id: {UserId}.", userId);

            var user = await _dbContext.Users.Include(u => u.UserCompanies)
                                                .ThenInclude(uc => uc.UserCompanyRoles)
                                                    .ThenInclude(ur => ur.Role)
                                             .AsNoTracking()
                                             .FirstOrDefaultAsync(d => d.Id == userId);
            if (user == null)
            {
                _logger.LogError("User not found for Id: {UserId}.", userId);

                return new("User not found", ResponseCode.NotFound);
            }

            _logger.LogInformation("User successfully retrieved for Id: {UserId}.", userId);

            return new(responseCode: ResponseCode.Ok)
            {
                Obj = _mapper.Map<UserDto>(user)
            };
        }

        public PagingDto<UserDto> RetrieveUsersPaging(PagingSearchInputBase input)
        {
            _logger.LogInformation("Starting to retrieve paginated list of users.");

            _logger.LogInformation("Pagination parameters - Page: {Page}, PageSize: {PageSize}, SearchKey: \"{SearchKey}\"",
                input.Page, input.PageSize, input.SearchKey ?? string.Empty);

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

            _logger.LogInformation("Successfully retrieved paginated list of users. Total items: {TotalItems}", retVal.RecordsTotal);

            return retVal;
        }

        public async Task<BaseDto> UpdateUserAsync(Guid userId, UserInput input)
        {
            _logger.LogInformation("Starting update user for Id: {UserId}.", userId);

            var (isValid, validationMessage) = await ValidateUserInput(input, userId);
            if (!isValid)
            {
                _logger.LogError("Failed update user. Reason: {ValidationMessage}.", validationMessage);

                return new(validationMessage, ResponseCode.Error);
            }

            var user = await _dbContext.Users.Include(u => u.UserCompanies).ThenInclude(uc => uc.UserCompanyRoles).FirstOrDefaultAsync(d => d.Id == userId);
            if (user == null)
            {
                _logger.LogError("User not found for Id: {UserId}.", userId);

                return new("User not found", ResponseCode.NotFound);
            }

            _mapper.Map(input, user);

            #region Delete mapping user company does not exists on input

            var deleteUserCompanies = from d in user.UserCompanies
                                      join i in input.UserCompanies on d.Id equals i.UserCompanyId into iLeft
                                      from i in iLeft.DefaultIfEmpty()
                                      where i == null
                                      select d;

            foreach (var deleteUserCompany in deleteUserCompanies)
            {
                deleteUserCompany.RowStatus = 1;
            }

            #region Edit mapping user company exists on input

            var editUserCompanies = from d in user.UserCompanies
                                    join i in input.UserCompanies on d.Id equals i.UserCompanyId
                                    select d;

            foreach (var editUserCompany in editUserCompanies)
            {
                _mapper.Map(input.UserCompanies.First(d => d.UserCompanyId == editUserCompany.Id), editUserCompany);

                var userCompanyInput = input.UserCompanies.First(d => d.UserCompanyId == editUserCompany.Id);

                #region Delete role does not exists on input

                var deleteRoles = from d in editUserCompany.UserCompanyRoles
                                  where !userCompanyInput.RoleIds.Contains(d.RoleId)
                                  select d;

                foreach (var deleteRole in deleteRoles)
                {
                    deleteRole.RowStatus = 1;
                }

                #endregion

                #region Add role does not exists on input

                var addRoles = from i in userCompanyInput.RoleIds
                               where !editUserCompany.UserCompanyRoles.Any(d => d.RoleId == i)
                               select i;

                if (addRoles.Any())
                    await _dbContext.UserCompanyRoles.AddRangeAsync(addRoles.Select(d => new UserCompanyRole { UserCompanyId = editUserCompany.Id, RoleId = d }));

                #endregion
            }

            #endregion

            #endregion

            #region Add role does not exists on input

            var addUserCompanies = from i in input.UserCompanies
                                   where !user.UserCompanies.Any(d => d.Id == i.UserCompanyId)
                                   select i;

            if (addUserCompanies.Any())
                await _dbContext.UserCompanies.AddRangeAsync(_mapper.Map<ICollection<UserCompany>>(addUserCompanies, opts => opts.Items["IsCreate"] = true));

            #endregion

            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("Successfully updated user for Id: {UserId}.", userId);

            return new("User successfully updated", ResponseCode.Ok);
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

            if (!input.UserCompanies.SelectMany(d => d.RoleIds).Any())
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
