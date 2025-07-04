using NET.Starter.Core.Services.Organization.Dtos;
using NET.Starter.Core.Services.Security.Dtos;
using NET.Starter.Core.Services.Security.Inputs;
using NET.Starter.Shared.Objects.Dtos;

namespace NET.Starter.Core.Services.Security.Interfaces
{
    public interface IAccountService
    {
        Task<ObjectDto<LoginDto>> LoginAsync(LoginInput input);

        Task<ObjectDto<LoginDto>> GoogleLoginAsync(GoogleLoginInput input);

        Task<ObjectDto<IEnumerable<CompanyDto>>> RetrieveMyCompaniesAsync();

        Task<ObjectDto<LoginDto>> RefreshTokenAsync(Guid? companyId = null);
    }
}
