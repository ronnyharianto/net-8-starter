using Microsoft.AspNetCore.Mvc;
using NET.Starter.API.Core.Services.Rfid;
using NET.Starter.API.Shared.Attributes;
using NET.Starter.API.Shared.Objects.Dtos;
using Swashbuckle.AspNetCore.Annotations;

namespace NET.Starter.API.Controllers.V1.Security
{
    [Route("api/v1/[controller]")]
    public class RfidController(RfidService rfidService) : BaseController
    {
        private readonly RfidService _rfidService = rfidService;

        // TO DO : Remove this example
        [AppAuthorize("Authorized")]
        [HttpPost("readtag")]
        [SwaggerOperation(Summary = "Dummy Endpoint 2", Description = "Untuk mencoba dependency injection dari beberapa merk reader")]
        public ResponseObject<string> ReadTag(string readerType)
        {
            return _rfidService.ReadTag(readerType);
        }
    }
}