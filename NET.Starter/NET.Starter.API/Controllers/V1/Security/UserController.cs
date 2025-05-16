using Microsoft.AspNetCore.Mvc;
using NET.Starter.Core.Services.Security.Dtos;
using NET.Starter.Core.Services.Security.Inputs;
using NET.Starter.Core.Services.Security.Interfaces;
using NET.Starter.Shared.Attributes;
using NET.Starter.Shared.Constants;
using NET.Starter.Shared.Objects.Dtos;
using NET.Starter.Shared.Objects.Inputs;
using Swashbuckle.AspNetCore.Annotations;

namespace NET.Starter.API.Controllers.V1.Security
{
    [Route("api/v1/[controller]")]
    public class UserController(IUserService userService) : BaseController
    {
        private readonly IUserService _userService = userService;

        [AppAuthorize(PermissionConstants.Security.User.View)]
        [HttpGet("paging")]
        [SwaggerOperation(Summary = "Retrieve paginated user")]
        public PagingDto<UserDto> RetrievePagingUser([FromQuery] PagingSearchInputBase input) => _userService.RetrieveUsersPaging(input);

        [AppAuthorize(PermissionConstants.Security.User.View)]
        [HttpGet("{userId:guid}")]
        [SwaggerOperation(Summary = "Retrieve user data by id")]
        public async Task<ObjectDto<UserDto>> RetrieveUserByIdAsync(Guid userId) => await _userService.RetrieveUserByIdAsync(userId);

        [AppAuthorize(PermissionConstants.Security.User.Create)]
        [Mutation]
        [HttpPost("create")]
        [SwaggerOperation(Summary = "Create user")]
        public async Task<BaseDto> CreateUserAsync([FromBody] UserInput input) => await _userService.CreateUserAsync(input);

        [AppAuthorize(PermissionConstants.Security.User.Update)]
        [Mutation]
        [HttpPut("update/{userId:guid}")]
        [SwaggerOperation(Summary = "Update user")]
        public async Task<BaseDto> UpdateUserAsync(Guid userId, [FromBody] UserInput input) => await _userService.UpdateUserAsync(userId, input);

        [AppAuthorize(PermissionConstants.Security.User.Update)]
        [Mutation]
        [HttpPatch("activate/{userId:guid}")]
        [SwaggerOperation(Summary = "Activate user")]
        public async Task<BaseDto> ActivateUserAsync(Guid userId) => await _userService.ActivateUserAsync(userId);

        [AppAuthorize(PermissionConstants.Security.User.Update)]
        [Mutation]
        [HttpPatch("deactivate/{userId:guid}")]
        [SwaggerOperation(Summary = "Deactivate user")]
        public async Task<BaseDto> DeactivateUserAsync(Guid userId) => await _userService.DeactivateUserAsync(userId);

        [AppAuthorize(PermissionConstants.Security.User.Delete)]
        [Mutation]
        [HttpDelete("delete/{userId:guid}")]
        [SwaggerOperation(Summary = "Delete user")]
        public async Task<BaseDto> DeleteUserAsync(Guid userId) => await _userService.DeleteUserAsync(userId);
    }
}
