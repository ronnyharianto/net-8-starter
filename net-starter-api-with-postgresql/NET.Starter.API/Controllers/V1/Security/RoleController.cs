using Microsoft.AspNetCore.Mvc;
using NET.Starter.Core.Services.Security.Dtos;
using NET.Starter.Core.Services.Security.Inputs;
using NET.Starter.Core.Services.Security.Interfaces;
using NET.Starter.Shared.Attributes;
using NET.Starter.Shared.Objects.Dtos;
using NET.Starter.Shared.Objects.Inputs;
using Swashbuckle.AspNetCore.Annotations;
using static NET.Starter.Shared.Constants.PermissionConstants.Security;

namespace NET.Starter.API.Controllers.V1.Security
{
    [Route("api/v1/[controller]")]
    public class RoleController(IRoleService _roleService) : BaseController
    {
        [AppAuthorize(Role.Access)]
        [HttpGet("all")]
        [SwaggerOperation(Summary = "Retrieve all role")]
        public async Task<ObjectDto<IEnumerable<RoleDto>>> RetrieveRolesAsync() => await _roleService.RetrieveRolesAsync();

        [AppAuthorize(Role.Access)]
        [HttpGet("paging")]
        [SwaggerOperation(Summary = "Retrieve paginated role")]
        [NonAction]
        public async Task<PagingDto<RoleDto>> RetrieveRolesPagingAsync([FromQuery] PagingSearchInputBase input) => await _roleService.RetrieveRolesPagingAsync(input);

        [AppAuthorize(Role.Access)]
        [HttpGet("{roleId:guid}")]
        [SwaggerOperation(Summary = "Retrieve role data by id")]
        [NonAction]
        public async Task<ObjectDto<RoleDto>> RetrieveRoleByIdAsync(Guid roleId) => await _roleService.RetrieveRoleByIdAsync(roleId);

        [AppAuthorize(Role.Modify)]
        [Mutation]
        [HttpPost("create")]
        [SwaggerOperation(Summary = "Create role")]
        [NonAction]
        public async Task<BaseDto> CreateRoleAsync([FromBody] RoleInput input) => await _roleService.CreateRoleAsync(input);

        [AppAuthorize(Role.Modify)]
        [Mutation]
        [HttpPut("update/{roleId:guid}")]
        [SwaggerOperation(Summary = "Update role")]
        [NonAction]
        public async Task<BaseDto> UpdateRoleAsync(Guid roleId, [FromBody] RoleInput input) => await _roleService.UpdateRoleAsync(roleId, input);

        [AppAuthorize(Role.Delete)]
        [Mutation]
        [HttpDelete("delete/{roleId:guid}")]
        [SwaggerOperation(Summary = "Delete role")]
        [NonAction]
        public async Task<BaseDto> DeleteRoleAsync(Guid roleId) => await _roleService.DeleteRoleAsync(roleId);
    }
}
