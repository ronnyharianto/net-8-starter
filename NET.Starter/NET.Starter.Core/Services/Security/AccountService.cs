using AutoMapper;
using Microsoft.Extensions.Logging;
using NET.Starter.Core.Bases;
using NET.Starter.Core.Services.Security.Dtos;
using NET.Starter.Core.Services.Security.Inputs;
using NET.Starter.DataAccess.SqlServer;
using NET.Starter.Shared.Enums;
using NET.Starter.Shared.Objects.Dtos;

namespace NET.Starter.Core.Services.Security
{
    public class AccountService(ApplicationDbContext dbContext, IMapper mapper, ILogger<AccountService> logger, TokenService tokenService)
        : BaseService<AccountService>(dbContext, mapper, logger)
    {
        private readonly TokenService _tokenService = tokenService;

        // TODO: Implement changes flow when login user duende identity & azure active directory
        public async Task<ObjectDto<TokenDto>> Login(LoginInput input)
        {
            var token = await _tokenService.GenerateToken("userName", Guid.NewGuid(), "fullName", input.Permissions);

            return new ObjectDto<TokenDto>(responseCode: ResponseCode.Ok)
            {
                Obj = token
            };
        }
    }
}
