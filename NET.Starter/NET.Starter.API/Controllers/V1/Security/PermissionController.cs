using Microsoft.AspNetCore.Mvc;
using NET.Starter.Core.Services.Security.Dtos;
using NET.Starter.Core.Services.Security.Interfaces;
using NET.Starter.Shared.Attributes;
using NET.Starter.Shared.Constants;
using NET.Starter.Shared.Objects.Dtos;
using Swashbuckle.AspNetCore.Annotations;

namespace NET.Starter.API.Controllers.V1.Security
{
    // Define the route for this controller as 'api/v1/Permission'
    [Route("api/v1/[controller]")]
    public class PermissionController(IPermissionService permissionService) : BaseController
    {
        private readonly IPermissionService _permissionService = permissionService;

        /// <summary>
        /// Retrieves a list of all permissions.
        /// </summary>
        /// <returns>An ObjectDto containing a list of PermissionDto.</returns>
        /// <remarks>
        /// - Requires the "View Permission" authorization.
        /// - Provides metadata for Swagger with operation summary and description.
        /// </remarks>
        [AppAuthorize(PermissionConstants.Security.Permission.View)]
        [HttpGet("all")]
        [SwaggerOperation(Summary = "Retrieve permissions")]
        public async Task<ObjectDto<IEnumerable<PermissionDto>>> RetrievePermissionsAsync()
            => await _permissionService.RetrievePermissionsAsync();
    }
}
