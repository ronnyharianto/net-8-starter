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
    public class RoleController(IRoleService roleService) : BaseController
    {
        private readonly IRoleService _roleService = roleService;

        [AppAuthorize(PermissionConstants.Security.Role.View)]
        [HttpGet("all")]
        [SwaggerOperation(Summary = "Retrieve roles")]
        public async Task<ObjectDto<IEnumerable<RoleDto>>> RetrieveRolesAsync() => await _roleService.RetrieveRolesAsync();

        [AppAuthorize(PermissionConstants.Security.Role.View)]
        [HttpGet("paging")]
        [SwaggerOperation(Summary = "Retrieve paginated roles")]
        public PagingDto<RoleDto> RetrievePagingRole([FromQuery] PagingSearchInputBase input) => _roleService.RetrieveRolesPaging(input);

        [AppAuthorize(PermissionConstants.Security.Role.View)]
        [HttpGet("{roleId:guid}")]
        [SwaggerOperation(Summary = "Retrieve role data by id")]
        public async Task<ObjectDto<RoleDto>> RetrieveRoleByIdAsync(Guid roleId) => await _roleService.RetrieveRoleByIdAsync(roleId);

        [AppAuthorize(PermissionConstants.Security.Role.Create)]
        [Mutation]
        [HttpPost("create")]
        [SwaggerOperation(Summary = "Create role")]
        public async Task<BaseDto> CreateRoleAsync([FromBody] RoleInput input) => await _roleService.CreateRoleAsync(input);

        [AppAuthorize(PermissionConstants.Security.Role.Update)]
        [Mutation]
        [HttpPut("update/{roleId:guid}")]
        [SwaggerOperation(Summary = "Update role")]
        public async Task<BaseDto> UpdateRoleAsync(Guid roleId, [FromBody] RoleInput input) => await _roleService.UpdateRoleAsync(roleId, input);

        [AppAuthorize(PermissionConstants.Security.Role.Delete)]
        [Mutation]
        [HttpDelete("delete/{roleId:guid}")]
        [SwaggerOperation(Summary = "Delete role")]
        public async Task<BaseDto> DeleteRoleAsync(Guid roleId) => await _roleService.DeleteRoleAsync(roleId);
    }
}
