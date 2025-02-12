using Microsoft.AspNetCore.Mvc;
using NET.Starter.Core.Services.Rfid;
using NET.Starter.Shared.Attributes;
using NET.Starter.Shared.Objects.Dtos;
using Swashbuckle.AspNetCore.Annotations;
using NET.Starter.SDK.Inputs;
using NET.Starter.SDK.Dtos;

namespace NET.Starter.API.Controllers.V1.Security
{
    [Route("api/v1/[controller]")]
    public class RfidController(RfidService rfidService) : BaseController
    {
        private readonly RfidService _rfidService = rfidService;

        // TO DO : Remove this example
        [AppAuthorize("Authorized")]
        [HttpPost("connect")]
        [SwaggerOperation(Summary = "Dummy Endpoint 2", Description = "Untuk mencoba dependency injection dari beberapa merk reader")]
        public ObjectDto<ConnectedInfoDto> Connect(ConnectInput input)
        {
            return _rfidService.Connect(input);
        }
    }
}