using Microsoft.AspNetCore.Mvc;
using NET.Starter.Core.Services.Security;
using NET.Starter.Core.Services.Security.Dtos;
using NET.Starter.Core.Services.Security.Inputs;
using NET.Starter.Shared.Attributes;
using NET.Starter.Shared.Constants;
using NET.Starter.Shared.Objects.Dtos;
using NET.Starter.Shared.Objects.Inputs;
using Swashbuckle.AspNetCore.Annotations;

namespace NET.Starter.API.Controllers.V1.Security
{
    // Route for this controller: 'api/v1/Role'
    [Route("api/v1/[controller]")]
    public class RoleController(RoleService roleService) : BaseController
    {
        // Field to hold the instance of the RoleService injected through the constructor
        private readonly RoleService _roleService = roleService;

        /// <summary>
        /// Retrieve all roles.
        /// </summary>
        /// <returns>An ObjectDto containing a list of RoleDto objects.</returns>
        [AppAuthorize(PermissionConstants.Security.Role.View)] // Requires 'View Role' permission
        [HttpGet("all")]
        [SwaggerOperation(Summary = "Retrieve roles", Description = "Retrieve all data of Role")]
        public async Task<ObjectDto<IEnumerable<RoleDto>>> RetrieveRolesAsync()
            => await _roleService.RetrieveRolesAsync();

        /// <summary>
        /// Retrieve roles with pagination.
        /// </summary>
        /// <param name="input">The paging and search criteria.</param>
        /// <returns>A PagingDto containing a paginated list of RoleDto objects.</returns>
        [AppAuthorize(PermissionConstants.Security.Role.View)] // Requires 'View Role' permission
        [HttpGet("paging")]
        [SwaggerOperation(Summary = "Retrieve roles with pagination", Description = "Retrieve a paginated list of Role")]
        public PagingDto<RoleDto> RetrievePagingRole([FromQuery] PagingSearchInputBase input)
            => _roleService.RetrieveRolesPaging(input);

        /// <summary>
        /// Retrieve a specific role by its ID.
        /// </summary>
        /// <param name="roleId">The ID of the role to retrieve.</param>
        /// <returns>An ObjectDto containing the RoleDto data.</returns>
        [AppAuthorize(PermissionConstants.Security.Role.View)] // Requires 'View Role' permission
        [HttpGet("{roleId:guid}")]
        [SwaggerOperation(Summary = "Retrieve role data by id", Description = "Retrieve role data for a specific id")]
        public async Task<ObjectDto<RoleDto>> RetrieveRoleByIdAsync(Guid roleId)
           => await _roleService.RetrieveRoleByIdAsync(roleId);

        /// <summary>
        /// Create a new role.
        /// </summary>
        /// <param name="input">The data required to create a role.</param>
        /// <returns>A BaseDto indicating the success or failure of the operation.</returns>
        [AppAuthorize(PermissionConstants.Security.Role.Create)] // Requires 'Create Role' permission
        [Mutation] // Marks this method as a mutation
        [HttpPost("create")]
        [SwaggerOperation(Summary = "Create role", Description = "Create a new role")]
        public async Task<BaseDto> CreateRoleAsync([FromBody] RoleInput input)
            => await _roleService.CreateRoleAsync(input);

        /// <summary>
        /// Update an existing role.
        /// </summary>
        /// <param name="input">The data required to update a role.</param>
        /// <returns>A BaseDto indicating the success or failure of the operation.</returns>
        [AppAuthorize(PermissionConstants.Security.Role.Update)] // Requires 'Update Role' permission
        [Mutation] // Marks this method as a mutation
        [HttpPut("update")]
        [SwaggerOperation(Summary = "Update role", Description = "Update an existing Role data")]
        public async Task<BaseDto> UpdateRoleAsync([FromBody] RoleInput input)
           => await _roleService.UpdateRoleAsync(input);

        /// <summary>
        /// Delete an existing role by its ID.
        /// </summary>
        /// <param name="roleId">The ID of the role to delete.</param>
        /// <returns>A BaseDto indicating the success or failure of the operation.</returns>
        [AppAuthorize(PermissionConstants.Security.Role.Delete)] // Requires 'Delete Role' permission
        [Mutation] // Marks this method as a mutation
        [HttpDelete("delete/{roleId:guid}")]
        [SwaggerOperation(Summary = "Delete role", Description = "Delete an existing Role data")]
        public async Task<BaseDto> DeleteRoleAsync(Guid roleId)
           => await _roleService.DeleteRoleAsync(roleId);
    }
}
