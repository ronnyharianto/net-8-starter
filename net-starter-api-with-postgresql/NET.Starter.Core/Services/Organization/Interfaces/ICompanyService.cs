using NET.Starter.Core.Services.Organization.Dtos;
using NET.Starter.Core.Services.Organization.Inputs;
using NET.Starter.Shared.Objects.Dtos;
using NET.Starter.Shared.Objects.Inputs;

namespace NET.Starter.Core.Services.Organization.Interfaces
{
    public interface ICompanyService
    {
        Task<ObjectDto<IEnumerable<CompanyDto>>> RetrieveCompaniesAsync();

        Task<PagingDto<CompanyDto>> RetrieveCompaniesPagingAsync(PagingSearchInputBase input);

        Task<ObjectDto<CompanyDto>> RetrieveCompanyByIdAsync(Guid companyId);

        Task<BaseDto> CreateCompanyAsync(CompanyInput input);

        Task<BaseDto> UpdateCompanyAsync(Guid companyId, CompanyInput input);

        Task<BaseDto> DeleteCompanyAsync(Guid companyId);
    }
}
