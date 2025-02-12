using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NET.Starter.Core.Services.Security;
using NET.Starter.Core.Services.Security.Dtos;
using NET.Starter.Core.Services.Security.Inputs;
using NET.Starter.Shared.Attributes;
using NET.Starter.Shared.Enums;
using NET.Starter.Shared.Objects.Dtos;
using Swashbuckle.AspNetCore.Annotations;

namespace NET.Starter.API.Controllers.V1.Security
{
    [Route("api/v1/[controller]")]
    public class AccountController(AccountService accountService) : BaseController
    {
        private readonly AccountService _accountService = accountService;

        #region AllowAnonymous
        // TODO: Implement changes flow when login user duende identity & azure active directory
        [AllowAnonymous]
        [HttpPost("login")]
        [SwaggerOperation(Summary = "Login", Description = "")]
        public async Task<ObjectDto<TokenDto>> GenerateToken(LoginInput input)
        {
            return await _accountService.Login(input);
        }
        #endregion

        // TO DO : Remove this example
        [AppAuthorize("Authorized")]
        [HttpPost("verify/authrorize")]
        [SwaggerOperation(Summary = "Dummy Endpoint 1", Description = "Untuk mencoba authorize atau tidak access token")]
        public BaseDto Authorized()
        {
            return new BaseDto(responseCode: ResponseCode.Ok);
        }
    }
}