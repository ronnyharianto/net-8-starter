using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NET.Starter.API.Core.Services.Security;
using NET.Starter.API.Core.Services.Security.Dtos;
using NET.Starter.API.Shared.Attributes;
using NET.Starter.API.Shared.Enums;
using NET.Starter.API.Shared.Objects.Dtos;

namespace NET.Starter.API.Controllers.V1.Security
{
    [Route("api/v1/[controller]")]
    public class TokenController(TokenService tokenService) : BaseController
    {
        private readonly TokenService _tokenService = tokenService;

        #region AllowAnonymous
        [AllowAnonymous]
        [HttpPost("generatetoken")]
        public async Task<ResponseObject<TokenDto>> GenerateToken()
        {
            return new ResponseObject<TokenDto>(responseCode: ResponseCode.Ok)
            {
                Obj = await _tokenService.GenerateToken("userName", Guid.NewGuid(), "fullName", [])
            };
        }
        #endregion

        [HttpPost("unauthrorize")]
        [AppAuthorize("UnAuthorized")]
        public ResponseBase UnAuthorized()
        {
            return new ResponseBase(responseCode: ResponseCode.Ok);
        }
    }
}

