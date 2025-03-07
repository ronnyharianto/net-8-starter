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
    // Route for this controller: 'api/v1/User'
    [Route("api/v1/[controller]")]
    public class UserController(IUserService userService) : BaseController
    {
        private readonly IUserService _userService = userService;

        /// <summary>
        /// Retrieve users with pagination.
        /// </summary>
        /// <param name="input">The paging and search criteria.</param>
        /// <returns>A PagingDto containing a paginated list of UserDto objects.</returns>
        [AppAuthorize(PermissionConstants.Security.User.View)]
        [HttpGet("paging")]
        [SwaggerOperation(Summary = "Retrieve paginated users")]
        public PagingDto<UserDto> RetrievePagingUser([FromQuery] PagingSearchInputBase input)
            => _userService.RetrieveUsersPaging(input);

        /// <summary>
        /// Retrieve a specific user by its ID.
        /// </summary>
        /// <param name="userId">The ID of the user to retrieve.</param>
        /// <returns>An ObjectDto containing the UserDto data.</returns>
        [AppAuthorize(PermissionConstants.Security.User.View)]
        [HttpGet("{userId:guid}")]
        [SwaggerOperation(Summary = "Retrieve user data by id")]
        public async Task<ObjectDto<UserDto>> RetrieveUserByIdAsync(Guid userId)
           => await _userService.RetrieveUserByIdAsync(userId);

        /// <summary>
        /// Create a new user.
        /// </summary>
        /// <param name="input">The data required to create a user.</param>
        /// <returns>A BaseDto indicating the success or failure of the operation.</returns>
        [AppAuthorize(PermissionConstants.Security.User.Create)]
        [Mutation]
        [HttpPost("create")]
        [SwaggerOperation(Summary = "Create user")]
        public async Task<BaseDto> CreateUserAsync([FromBody] UserInput input)
            => await _userService.CreateUserAsync(input);

        /// <summary>
        /// Update an existing user.
        /// </summary>
        /// <param name="input">The data required to update a user.</param>
        /// <returns>A BaseDto indicating the success or failure of the operation.</returns>
        [AppAuthorize(PermissionConstants.Security.User.Update)]
        [Mutation]
        [HttpPut("update/{userId:guid}")]
        [SwaggerOperation(Summary = "Update user")]
        public async Task<BaseDto> UpdateUserAsync(Guid userId, [FromBody] UserInput input)
           => await _userService.UpdateUserAsync(userId, input);

        /// <summary>
        /// Activate an existing user.
        /// </summary>
        /// <param name="userId">The ID of the user to activate.</param>
        /// <returns>A BaseDto indicating the success or failure of the operation.</returns>
        [AppAuthorize(PermissionConstants.Security.User.Update)]
        [Mutation]
        [HttpPatch("activate/{userId:guid}")]
        [SwaggerOperation(Summary = "Activate user")]
        public async Task<BaseDto> ActivateUserAsync(Guid userId)
           => await _userService.ActivateUserAsync(userId);

        /// <summary>
        /// Deactivate an existing user.
        /// </summary>
        /// <param name="userId">The ID of the user to deactivate.</param>
        /// <returns>A BaseDto indicating the success or failure of the operation.</returns>
        [AppAuthorize(PermissionConstants.Security.User.Update)]
        [Mutation]
        [HttpPatch("deactivate/{userId:guid}")]
        [SwaggerOperation(Summary = "Deactivate user")]
        public async Task<BaseDto> DeactivateUserAsync(Guid userId)
           => await _userService.DeactivateUserAsync(userId);

        /// <summary>
        /// Delete an existing user by its ID.
        /// </summary>
        /// <param name="userId">The ID of the user to delete.</param>
        /// <returns>A BaseDto indicating the success or failure of the operation.</returns>
        [AppAuthorize(PermissionConstants.Security.User.Delete)]
        [Mutation]
        [HttpDelete("delete/{userId:guid}")]
        [SwaggerOperation(Summary = "Delete user")]
        public async Task<BaseDto> DeleteUserAsync(Guid userId)
           => await _userService.DeleteUserAsync(userId);
    }
}
