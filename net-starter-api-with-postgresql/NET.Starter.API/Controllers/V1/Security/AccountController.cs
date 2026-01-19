using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NET.Starter.Core.Services.Organization.Dtos;
using NET.Starter.Core.Services.Security.Dtos;
using NET.Starter.Core.Services.Security.Inputs;
using NET.Starter.Core.Services.Security.Interfaces;
using NET.Starter.Shared.Attributes;
using NET.Starter.Shared.Constants;
using NET.Starter.Shared.Objects.Dtos;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace NET.Starter.API.Controllers.V1.Security
{
    public class AccountController(IAccountService _accountService) : BaseController
    {
        [AllowAnonymous]
        [Mutation([HttpStatusCode.OK, HttpStatusCode.Unauthorized])]
        [HttpPost("login")]
        [SwaggerOperation(Summary = "Login account")]
        public async Task<ObjectDto<LoginDto>> LoginAsync(LoginInput input) => await _accountService.LoginAsync(input);

        [AllowAnonymous]
        [Mutation]
        [HttpPost("login/google")]
        [SwaggerOperation(Summary = "Login account with google credential")]
        public async Task<ObjectDto<LoginDto>> GoogleLoginAsync(GoogleLoginInput input) => await _accountService.GoogleLoginAsync(input);

        [AppAuthorize(PermissionConstant.RefreshToken)]
        [HttpGet("refresh-token")]
        [SwaggerOperation(Summary = "Refresh token")]
        public async Task<ObjectDto<LoginDto>> RefreshTokenAsync() => await _accountService.RefreshTokenAsync();

        [AppAuthorize]
        [HttpGet("my-companies")]
        [SwaggerOperation(Summary = "Retrieve my companies")]
        public async Task<ObjectDto<IEnumerable<CompanyDto>>> RetrieveMyCompaniesAsync() => await _accountService.RetrieveMyCompaniesAsync();

        [AppAuthorize]
        [HttpGet("change-company/{companyId:guid}")]
        [SwaggerOperation(Summary = "Change access company")]
        public async Task<ObjectDto<LoginDto>> ChangeCompanyAsync(Guid companyId)
        {
            var tokenResult = await _accountService.RefreshTokenAsync(companyId);

            if (!tokenResult.Succeeded) tokenResult.Message = "Changing company failed because user not found.";

            return tokenResult;
        }
    }
}
