using Microsoft.AspNetCore.Mvc;
using NET.Starter.Core.Services.Organization.Dtos;
using NET.Starter.Core.Services.Organization.Inputs;
using NET.Starter.Core.Services.Organization.Interfaces;
using NET.Starter.Shared.Attributes;
using NET.Starter.Shared.Objects.Dtos;
using NET.Starter.Shared.Objects.Inputs;
using Swashbuckle.AspNetCore.Annotations;
using static NET.Starter.Shared.Constants.PermissionConstants.Organization;
using UserPermission = NET.Starter.Shared.Constants.PermissionConstants.Security.User;

namespace NET.Starter.API.Controllers.V1.Organization
{
    [Route("api/v1/[controller]")]
    public class CompanyController(ICompanyService _companyService) : BaseController
    {
        [AppAuthorize(Company.Access, UserPermission.Modify)]
        [HttpGet("all")]
        [SwaggerOperation(Summary = "Retrieve all company")]
        public async Task<ObjectDto<IEnumerable<CompanyDto>>> RetrieveCompaniesAsync() => await _companyService.RetrieveCompaniesAsync();

        [AppAuthorize(Company.Access)]
        [HttpGet("paging")]
        [SwaggerOperation(Summary = "Retrieve paginated company")]
        public async Task<PagingDto<CompanyDto>> RetrieveCompaniesPagingAsync([FromQuery] PagingSearchInputBase input) => await _companyService.RetrieveCompaniesPagingAsync(input);

        [AppAuthorize(Company.Access)]
        [HttpGet("{companyId:guid}")]
        [SwaggerOperation(Summary = "Retrieve company data by id")]
        public async Task<ObjectDto<CompanyDto>> RetrieveCompanyByIdAsync(Guid companyId) => await _companyService.RetrieveCompanyByIdAsync(companyId);

        [AppAuthorize(Company.Modify)]
        [Mutation]
        [HttpPost("create")]
        [SwaggerOperation(Summary = "Create company")]
        public async Task<BaseDto> CreateCompanyAsync([FromBody] CompanyInput input) => await _companyService.CreateCompanyAsync(input);

        [AppAuthorize(Company.Modify)]
        [Mutation]
        [HttpPut("update/{companyId:guid}")]
        [SwaggerOperation(Summary = "Update company")]
        public async Task<BaseDto> UpdateCompanyAsync(Guid companyId, [FromBody] CompanyInput input) => await _companyService.UpdateCompanyAsync(companyId, input);

        [AppAuthorize(Company.Delete)]
        [Mutation]
        [HttpDelete("delete/{companyId:guid}")]
        [SwaggerOperation(Summary = "Delete company")]
        public async Task<BaseDto> DeleteCompanyAsync(Guid companyId) => await _companyService.DeleteCompanyAsync(companyId);
    }
}
