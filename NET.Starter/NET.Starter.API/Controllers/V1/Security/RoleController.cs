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
    // Route for this controller: 'api/v1/Role'
    [Route("api/v1/[controller]")]
    public class RoleController(IRoleService roleService) : BaseController
    {
        private readonly IRoleService _roleService = roleService;

        /// <summary>
        /// Retrieve all roles.
        /// </summary>
        /// <returns>An ObjectDto containing a list of RoleDto objects.</returns>
        [AppAuthorize(PermissionConstants.Security.Role.View)]
        [HttpGet("all")]
        [SwaggerOperation(Summary = "Retrieve roles")]
        public async Task<ObjectDto<IEnumerable<RoleDto>>> RetrieveRolesAsync()
            => await _roleService.RetrieveRolesAsync();

        /// <summary>
        /// Retrieve roles with pagination.
        /// </summary>
        /// <param name="input">The paging and search criteria.</param>
        /// <returns>A PagingDto containing a paginated list of RoleDto objects.</returns>
        [AppAuthorize(PermissionConstants.Security.Role.View)]
        [HttpGet("paging")]
        [SwaggerOperation(Summary = "Retrieve paginated roles")]
        public PagingDto<RoleDto> RetrievePagingRole([FromQuery] PagingSearchInputBase input)
            => _roleService.RetrieveRolesPaging(input);

        /// <summary>
        /// Retrieve a specific role by its ID.
        /// </summary>
        /// <param name="roleId">The ID of the role to retrieve.</param>
        /// <returns>An ObjectDto containing the RoleDto data.</returns>
        [AppAuthorize(PermissionConstants.Security.Role.View)]
        [HttpGet("{roleId:guid}")]
        [SwaggerOperation(Summary = "Retrieve role data by id")]
        public async Task<ObjectDto<RoleDto>> RetrieveRoleByIdAsync(Guid roleId)
           => await _roleService.RetrieveRoleByIdAsync(roleId);

        /// <summary>
        /// Create a new role.
        /// </summary>
        /// <param name="input">The data required to create a role.</param>
        /// <returns>A BaseDto indicating the success or failure of the operation.</returns>
        [AppAuthorize(PermissionConstants.Security.Role.Create)]
        [Mutation]
        [HttpPost("create")]
        [SwaggerOperation(Summary = "Create role")]
        public async Task<BaseDto> CreateRoleAsync([FromBody] RoleInput input)
            => await _roleService.CreateRoleAsync(input);

        /// <summary>
        /// Update an existing role.
        /// </summary>
        /// <param name="input">The data required to update a role.</param>
        /// <returns>A BaseDto indicating the success or failure of the operation.</returns>
        [AppAuthorize(PermissionConstants.Security.Role.Update)]
        [Mutation]
        [HttpPut("update/{roleId:guid}")]
        [SwaggerOperation(Summary = "Update role")]
        public async Task<BaseDto> UpdateRoleAsync(Guid roleId, [FromBody] RoleInput input)
           => await _roleService.UpdateRoleAsync(roleId, input);

        /// <summary>
        /// Delete an existing role by its ID.
        /// </summary>
        /// <param name="roleId">The ID of the role to delete.</param>
        /// <returns>A BaseDto indicating the success or failure of the operation.</returns>
        [AppAuthorize(PermissionConstants.Security.Role.Delete)]
        [Mutation]
        [HttpDelete("delete/{roleId:guid}")]
        [SwaggerOperation(Summary = "Delete role")]
        public async Task<BaseDto> DeleteRoleAsync(Guid roleId)
           => await _roleService.DeleteRoleAsync(roleId);
    }
}
