using Microsoft.AspNetCore.Mvc;
using NET.Starter.Core.Services.Security.Dtos;
using NET.Starter.Core.Services.Security.Interfaces;
using NET.Starter.Shared.Attributes;
using NET.Starter.Shared.Constants;
using NET.Starter.Shared.Objects.Dtos;
using Swashbuckle.AspNetCore.Annotations;

namespace NET.Starter.API.Controllers.V1.Security
{
    [Route("api/v1/[controller]")]
    public class PermissionController(IPermissionService permissionService) : BaseController
    {
        private readonly IPermissionService _permissionService = permissionService;

        [AppAuthorize(PermissionConstants.Security.Permission.View)]
        [HttpGet("all")]
        [SwaggerOperation(Summary = "Retrieve all permission")]
        public async Task<ObjectDto<IEnumerable<PermissionDto>>> RetrievePermissionsAsync()
            => await _permissionService.RetrievePermissionsAsync();
    }
}
