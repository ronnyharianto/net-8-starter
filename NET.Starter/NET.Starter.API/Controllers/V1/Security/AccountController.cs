using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NET.Starter.Core.Services.Security;
using NET.Starter.Core.Services.Security.Dtos;
using NET.Starter.Core.Services.Security.Inputs;
using NET.Starter.Core.Services.Security.Interfaces;
using NET.Starter.Shared.Objects.Dtos;
using Swashbuckle.AspNetCore.Annotations;

namespace NET.Starter.API.Controllers.V1.Security
{
    // Define the route for this controller as 'api/v1/Account'
    [Route("api/v1/[controller]")]
    public class AccountController(IAccountService accountService) : BaseController
    {
        // Field to hold the instance of the AccountService injected through the constructor
        private readonly IAccountService _accountService = accountService;

        [AllowAnonymous]
        [HttpPost("login")]
        [SwaggerOperation(Summary = "Login account")]
        public async Task<ObjectDto<LoginDto>> LoginAsync(LoginInput input)
        {
            // Call the service layer to retrieve the data
            return await _accountService.LoginAsync(input);
        }
    }
}
