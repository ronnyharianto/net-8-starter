using Microsoft.AspNetCore.Mvc;
using NET.Starter.Core.Services.Security;
using NET.Starter.Core.Services.Security.Dtos;
using NET.Starter.Shared.Attributes;
using NET.Starter.Shared.Constants;
using NET.Starter.Shared.Objects.Dtos;
using Swashbuckle.AspNetCore.Annotations;

namespace NET.Starter.API.Controllers.V1.Security
{
    // Define the route for this controller as 'api/v1/Permission'
    [Route("api/v1/[controller]")]
    public class PermissionController(PermissionService permissionService) : BaseController
    {
        // Field to hold the instance of the PermissionService injected through the constructor
        private readonly PermissionService _permissionService = permissionService;

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
        [SwaggerOperation(Summary = "Retrieve permissions", Description = "Retrieve all data of Permission")]
        public async Task<ObjectDto<IEnumerable<PermissionDto>>> RetrievePermissionsAsync()
        {
            // Call the service layer to retrieve the data
            return await _permissionService.RetrievePermissionsAsync();
        }
    }
}
