using AutoMapper;
using Microsoft.Extensions.Logging;
using NET.Starter.API.Core.Bases;
using NET.Starter.API.Core.Services.Security.Dtos;
using NET.Starter.API.Core.Services.Security.Inputs;
using NET.Starter.API.DataAccess.SqlServer;
using NET.Starter.API.Shared.Enums;
using NET.Starter.API.Shared.Objects.Dtos;

namespace NET.Starter.API.Core.Services.Security
{
    public class AccountService(ApplicationDbContext dbContext, IMapper mapper, ILogger<AccountService> logger, TokenService tokenService)
        : BaseService<AccountService>(dbContext, mapper, logger)
    {
        private readonly TokenService _tokenService = tokenService;

        // TODO: Implement changes flow when login user duende identity & azure active directory
        public async Task<ResponseObject<TokenDto>> Login(LoginInput input)
        {
            var token = await _tokenService.GenerateToken("userName", Guid.NewGuid(), "fullName", input.Permissions);

            return new ResponseObject<TokenDto>(responseCode: ResponseCode.Ok)
            {
                Obj = token
            };
        }
    }
}
