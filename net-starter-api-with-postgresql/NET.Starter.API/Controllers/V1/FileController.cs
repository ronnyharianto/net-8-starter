using Microsoft.AspNetCore.Mvc;
using NET.Starter.Shared.Attributes;
using NET.Starter.Shared.Constants;
using NET.Starter.Shared.Helpers;
using NET.Starter.Shared.Objects.Dtos;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace NET.Starter.API.Controllers.V1
{
    [Route("api/v1/[controller]")]
    public class FileController() : BaseController
    {
        [AppAuthorize(PermissionConstant.RetrieveFileFromStorage)]
        [HttpGet("retrieve-signed-url")]
        [SwaggerOperation(Summary = "Retrieve limited time signed url")]
        public async Task<ObjectDto<string>> RetrieveBrandsPagingAsync([FromQuery] string objectName, [FromQuery] int? durationInSeconds)
        {
            var signedUrl = await GoogleCloudStorageHelper.RetrieveSignedUrlFile(objectName, durationInSeconds ?? 60);

            return new(httpStatusCode: HttpStatusCode.OK)
            {
                Obj = signedUrl
            };
        }
    }
}
