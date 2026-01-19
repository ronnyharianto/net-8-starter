using Microsoft.AspNetCore.Mvc;
using NET.Starter.Core.Services.Security.Dtos;
using NET.Starter.Core.Services.Security.Interfaces;
using NET.Starter.Shared.Attributes;
using NET.Starter.Shared.Objects.Dtos;
using Swashbuckle.AspNetCore.Annotations;
using static NET.Starter.Shared.Constants.PermissionConstant.Security;

namespace NET.Starter.API.Controllers.V1.Security
{
    public class PermissionController(IPermissionService _permissionService) : BaseController
    {
        [AppAuthorize(Role.Modify)]
        [HttpGet("all")]
        [SwaggerOperation(Summary = "Retrieve all permission")]
        public async Task<ObjectDto<IEnumerable<PermissionDto>>> RetrievePermissionsAsync()
            => await _permissionService.RetrievePermissionsAsync();
    }
}
