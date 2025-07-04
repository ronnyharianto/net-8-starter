using NET.Starter.Core.Services.Security.Dtos;
using NET.Starter.Core.Services.Security.Inputs;
using NET.Starter.Shared.Objects.Dtos;

namespace NET.Starter.Core.Services.Security.Interfaces
{
    public interface IUserService
    {
        Task<PagingDto<UserDto>> RetrieveUsersPagingAsync(PagingUserInput input);

        Task<ObjectDto<UserDto>> RetrieveUserByIdAsync(Guid userId);

        Task<BaseDto> CreateUserAsync(UserInput input);

        Task<BaseDto> UpdateUserAsync(Guid userId, UserInput input);

        Task<BaseDto> DeleteUserAsync(Guid userId);

        Task<BaseDto> ActivateUserAsync(Guid userId);

        Task<BaseDto> DeactivateUserAsync(Guid userId);

        Task<BaseDto> RegisterMyPushTokenAsync(UserPushTokenInput input);

        Task<BaseDto> UnregisterMyPushTokenAsync(UserPushTokenInput input);
    }
}
