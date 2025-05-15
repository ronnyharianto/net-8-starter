using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NET.Starter.Core.Services.Organization.Dtos;
using NET.Starter.Core.Services.Security.CustomModels;
using NET.Starter.Core.Services.Security.Inputs;
using NET.Starter.Core.Services.Security.Interfaces;
using NET.Starter.Shared.Attributes;
using NET.Starter.Shared.Constants;
using NET.Starter.Shared.Objects.Dtos;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace NET.Starter.API.Controllers.V1.Security
{
    [Route("api/v1/[controller]")]
    public class AccountController(IAccountService accountService) : BaseController
    {
        private readonly IAccountService _accountService = accountService;

        [AllowAnonymous]
        [Mutation([HttpStatusCode.OK, HttpStatusCode.Unauthorized])]
        [HttpPost("login")]
        [SwaggerOperation(Summary = "Login account")]
        public async Task<ObjectDto<TokenResult>> LoginAsync(LoginInput input) => await _accountService.LoginAsync(input);

        [AppAuthorize(PermissionConstants.RefreshToken)]
        [HttpGet("refresh-token")]
        [SwaggerOperation(Summary = "Refresh token")]
        public async Task<ObjectDto<TokenResult>> RefreshTokenAsync() => await _accountService.RefreshTokenAsync();

        [AppAuthorize]
        [HttpGet("my-companies")]
        [SwaggerOperation(Summary = "Retrieve my companies")]
        public async Task<ObjectDto<IEnumerable<CompanyDto>>> RetrieveMyCompaniesAsync() => await _accountService.RetrieveMyCompaniesAsync();

        [AppAuthorize]
        [HttpGet("change-company/{companyId:guid}")]
        [SwaggerOperation(Summary = "Change company")]
        public async Task<ObjectDto<TokenResult>> ChangeCompanyAsync(Guid companyId)
        {
            var tokenResult = await _accountService.RefreshTokenAsync(companyId);

            if (!tokenResult.Succeeded) tokenResult.Message = "Changing company failed because user not found.";

            return tokenResult;
        }
    }
}
