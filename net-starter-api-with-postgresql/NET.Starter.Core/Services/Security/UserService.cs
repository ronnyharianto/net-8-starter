using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NET.Starter.Core.Services.Security.Dtos;
using NET.Starter.Core.Services.Security.Inputs;
using NET.Starter.Core.Services.Security.Interfaces;
using NET.Starter.DataAccess;
using NET.Starter.DataAccess.Models.Security;
using NET.Starter.Shared.Objects;
using NET.Starter.Shared.Objects.Dtos;
using System.Net;

namespace NET.Starter.Core.Services.Security
{
    internal class UserService(ApplicationDbContext _dbContext, IMapper _mapper, ILogger<UserService> _logger, CurrentUserAccessor _currentUserAccessor) : IUserService
    {
        public async Task<PagingDto<UserDto>> RetrieveUsersPagingAsync(PagingUserInput input)
        {
            _logger.LogInformation("Starting to retrieve paginated list of users. Page: {Page}, PageSize: {PageSize}, SearchKey: \"{SearchKey}\"",
                input.Page, input.PageSize, input.SearchKey ?? string.Empty);

            var retVal = new PagingDto<UserDto>();

            var searchKey = input.SearchKey?.Trim() ?? string.Empty;
            var searchPattern = $"%{searchKey}%";

            var users = _dbContext.Users.AsNoTracking()
                                        .Where(d => input.CompanyId == null || d.UserCompanies.Any(uc => uc.CompanyId == input.CompanyId))
                                        .Where(d =>
                                            EF.Functions.ILike(d.FullName, searchPattern) 
                                            || EF.Functions.ILike(d.EmailAddress, searchPattern)
                                        )
                                        .OrderByDescending(d => d.Modified ?? d.Created)
                                        .ThenBy(d => d.EmailAddress)
                                        .Select(d => _mapper.Map<UserDto>(d));

            await retVal.ApplyPagination(input.Page, input.PageSize, users);

            _logger.LogInformation("Successfully retrieved {Count} users.", retVal.RecordsFiltered);

            return retVal;
        }

        public async Task<ObjectDto<UserDto>> RetrieveUserByIdAsync(Guid userId)
        {
            _logger.LogInformation("Starting to retrieve user with ID: {UserId}", userId);

            var user = await _dbContext.Users.Include(u => u.UserCompanies)
                                                .ThenInclude(uc => uc.UserCompanyRoles)
                                                    .ThenInclude(ur => ur.Role)
                                             .AsNoTracking()
                                             .FirstOrDefaultAsync(d => d.Id == userId);
            if (user == null)
            {
                _logger.LogWarning("User not found with ID: {UserId}", userId);
                return new("User not found", HttpStatusCode.NotFound);
            }

            _logger.LogInformation("User successfully retrieved with ID: {UserId}", userId);
            return new(httpStatusCode: HttpStatusCode.OK)
            {
                Obj = _mapper.Map<UserDto>(user)
            };
        }

        public async Task<BaseDto> CreateUserAsync(UserInput input)
        {
            _logger.LogInformation("Creating user with email: {Email}", input.EmailAddress);

            var (isValid, validationMessage) = await ValidateUserInput(input);
            if (!isValid)
            {
                _logger.LogWarning("User creation failed. Reason: {Message}", validationMessage);
                return new(validationMessage, HttpStatusCode.InternalServerError);
            }

            var user = _mapper.Map<User>(input, opts => opts.Items["IsCreate"] = true);

            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("User created successfully with email: {Email}", user.EmailAddress);
            return new("User has been successfully created.", HttpStatusCode.OK);
        }

        public async Task<BaseDto> UpdateUserAsync(Guid userId, UserInput input)
        {
            _logger.LogInformation("Updating user with ID: {UserId}", userId);

            var (isValid, validationMessage) = await ValidateUserInput(input, userId);
            if (!isValid)
            {
                _logger.LogWarning("User update failed. Reason: {Message}", validationMessage);
                return new(validationMessage, HttpStatusCode.InternalServerError);
            }

            var user = await _dbContext.Users.Include(u => u.UserCompanies).ThenInclude(uc => uc.UserCompanyRoles).FirstOrDefaultAsync(d => d.Id == userId);
            if (user == null)
            {
                _logger.LogWarning("User not found with ID: {UserId}", userId);
                return new("User not found", HttpStatusCode.NotFound);
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

            #endregion

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

            #region Add role does not exists on input

            var addUserCompanies = from i in input.UserCompanies
                                   where !user.UserCompanies.Any(d => d.Id == i.UserCompanyId)
                                   select i;

            if (addUserCompanies.Any())
                await _dbContext.UserCompanies.AddRangeAsync(_mapper.Map<ICollection<UserCompany>>(addUserCompanies, opts =>
                {
                    opts.Items["IsCreate"] = true;
                    opts.Items["UserId"] = user.Id;
                }));

            #endregion

            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("User updated successfully with ID: {UserId}", userId);
            return new("User successfully updated", HttpStatusCode.OK);
        }

        public async Task<BaseDto> DeleteUserAsync(Guid userId)
        {
            _logger.LogInformation("Deleting user with ID: {UserId}", userId);

            var user = await _dbContext.Users.FirstOrDefaultAsync(d => d.Id == userId);
            if (user == null)
            {
                _logger.LogWarning("User not found with ID: {UserId}", userId);
                return new("User not found.", HttpStatusCode.NotFound);
            }

            user.RowStatus = 1;

            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("User deleted successfully with ID: {UserId}", userId);
            return new("User has been successfully deleted.", HttpStatusCode.OK);
        }

        public async Task<BaseDto> ActivateUserAsync(Guid userId)
        {
            _logger.LogInformation("Activating user with ID: {UserId}", userId);

            var user = await _dbContext.Users.FirstOrDefaultAsync(d => d.Id == userId);
            if (user == null)
            {
                _logger.LogWarning("User not found with ID: {UserId}", userId);
                return new("User not found.", HttpStatusCode.NotFound);
            }

            user.IsActive = true;

            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("User activated with ID: {UserId}", userId);
            return new("User has been successfully activated.", HttpStatusCode.OK);
        }

        public async Task<BaseDto> DeactivateUserAsync(Guid userId)
        {
            _logger.LogInformation("Deactivating user with ID: {UserId}", userId);

            var user = await _dbContext.Users.FirstOrDefaultAsync(d => d.Id == userId);
            if (user == null)
            {
                _logger.LogWarning("User not found with ID: {UserId}", userId);
                return new("User not found.", HttpStatusCode.NotFound);
            }

            user.IsActive = false;

            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("User deactivated with ID: {UserId}", userId);
            return new("User has been successfully deactivated.", HttpStatusCode.OK);
        }

        public async Task<BaseDto> RegisterMyPushTokenAsync(UserPushTokenInput input)
        {
            _logger.LogInformation("Registering push token: {PushToken} for User: {UserId}", input.PushToken, _currentUserAccessor.UserId);

            var user = await _dbContext.Users.FirstOrDefaultAsync(d => d.Id == _currentUserAccessor.UserId);
            if (user == null)
            {
                _logger.LogWarning("Current user not found.");
                return new("User not found.", HttpStatusCode.NotFound);
            }

            var pushToken = await _dbContext.UserPushTokens.FirstOrDefaultAsync(d => d.PushToken == input.PushToken);
            if (pushToken != null)
            {
                pushToken.UserId = user.Id;
            }
            else
            {
                pushToken = _mapper.Map<UserPushToken>(input);
                pushToken.UserId = user.Id;

                await _dbContext.UserPushTokens.AddAsync(pushToken);
            }
                
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("Push token registered: {PushToken}", input.PushToken);
            return new("User Push token has been successfully registered.", HttpStatusCode.OK);
        }

        public async Task<BaseDto> UnregisterMyPushTokenAsync(UserPushTokenInput input)
        {
            _logger.LogInformation("Unregistering push token: {PushToken}", input.PushToken);

            var pushToken = await _dbContext.UserPushTokens.FirstOrDefaultAsync(d => d.PushToken == input.PushToken);
            if (pushToken == null)
            {
                _logger.LogWarning("Push token not found: {PushToken}", input.PushToken);
                return new("User Push token not found.", HttpStatusCode.NotFound);
            }

            pushToken.RowStatus = 1;

            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("Push token unregistered: {PushToken}", input.PushToken);
            return new("User Push token has been successfully unregistered.", HttpStatusCode.OK);
        }

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
            var dataDuplicateUser = await _dbContext.Users.FirstOrDefaultAsync(d =>
                (d.Username == input.EmailAddress || d.EmailAddress == input.EmailAddress) &&
                d.Id != userId
            );
            if (dataDuplicateUser != null)
                return (false, "Email address already exists.");

            return (true, string.Empty);
        }
    }
}
