using Microsoft.AspNetCore.Mvc;
using NET.Starter.Core.Services.Security.Dtos;
using NET.Starter.Core.Services.Security.Inputs;
using NET.Starter.Core.Services.Security.Interfaces;
using NET.Starter.Shared.Attributes;
using NET.Starter.Shared.Objects.Dtos;
using Swashbuckle.AspNetCore.Annotations;
using UserPermission = NET.Starter.Shared.Constants.PermissionConstants.Security.User;

namespace NET.Starter.API.Controllers.V1.Security
{
    [Route("api/v1/[controller]")]
    public class UserController(IUserService _userService) : BaseController
    {
        [AppAuthorize(UserPermission.Access)]
        [HttpGet("paging")]
        [SwaggerOperation(Summary = "Retrieve paginated user")]
        public async Task<PagingDto<UserDto>> RetrieveUsersPagingAsync([FromQuery] PagingUserInput input) => await _userService.RetrieveUsersPagingAsync(input);

        [AppAuthorize(UserPermission.Access)]
        [HttpGet("{userId:guid}")]
        [SwaggerOperation(Summary = "Retrieve user data by id")]
        public async Task<ObjectDto<UserDto>> RetrieveUserByIdAsync(Guid userId) => await _userService.RetrieveUserByIdAsync(userId);

        [AppAuthorize(UserPermission.Modify)]
        [Mutation]
        [HttpPost("create")]
        [SwaggerOperation(Summary = "Create user")]
        public async Task<BaseDto> CreateUserAsync([FromBody] UserInput input) => await _userService.CreateUserAsync(input);

        [AppAuthorize(UserPermission.Modify)]
        [Mutation]
        [HttpPut("update/{userId:guid}")]
        [SwaggerOperation(Summary = "Update user")]
        public async Task<BaseDto> UpdateUserAsync(Guid userId, [FromBody] UserInput input) => await _userService.UpdateUserAsync(userId, input);

        [AppAuthorize(UserPermission.Modify)]
        [Mutation]
        [HttpPatch("activate/{userId:guid}")]
        [SwaggerOperation(Summary = "Activate user")]
        public async Task<BaseDto> ActivateUserAsync(Guid userId) => await _userService.ActivateUserAsync(userId);

        [AppAuthorize(UserPermission.Modify)]
        [Mutation]
        [HttpPatch("deactivate/{userId:guid}")]
        [SwaggerOperation(Summary = "Deactivate user")]
        public async Task<BaseDto> DeactivateUserAsync(Guid userId) => await _userService.DeactivateUserAsync(userId);

        [AppAuthorize(UserPermission.Delete)]
        [Mutation]
        [HttpDelete("delete/{userId:guid}")]
        [SwaggerOperation(Summary = "Delete user")]
        public async Task<BaseDto> DeleteUserAsync(Guid userId) => await _userService.DeleteUserAsync(userId);

        [AppAuthorize]
        [Mutation]
        [HttpPost("register-fcm-token")]
        [SwaggerOperation(Summary = "Register fcm token for user")]
        public async Task<BaseDto> RegisterMyFcmTokenAsync(UserPushTokenInput input) => await _userService.RegisterMyPushTokenAsync(input);

        [AppAuthorize]
        [Mutation]
        [HttpDelete("unregister-fcm-token")]
        [SwaggerOperation(Summary = "Unregister fcm token for user")]
        public async Task<BaseDto> UnregisterMyFcmTokenAsync(UserPushTokenInput input) => await _userService.UnregisterMyPushTokenAsync(input);
    }
}
