using Microsoft.AspNetCore.Mvc;
using NET.Starter.Core.Services.Organization.Dtos;
using NET.Starter.Core.Services.Organization.Inputs;
using NET.Starter.Core.Services.Security.Interfaces;
using NET.Starter.Shared.Attributes;
using NET.Starter.Shared.Constants;
using NET.Starter.Shared.Objects.Dtos;
using NET.Starter.Shared.Objects.Inputs;
using Swashbuckle.AspNetCore.Annotations;

namespace NET.Starter.API.Controllers.V1.Organization
{
    [Route("api/v1/[controller]")]
    public class CompanyController(ICompanyService companyService) : BaseController
    {
        private readonly ICompanyService _companyService = companyService;

        [AppAuthorize(PermissionConstants.Organization.Company.View)]
        [HttpGet("all")]
        [SwaggerOperation(Summary = "Retrieve all company")]
        public async Task<ObjectDto<IEnumerable<CompanyDto>>> RetrieveCompaniesAsync() => await _companyService.RetrieveCompaniesAsync();

        [AppAuthorize(PermissionConstants.Organization.Company.View)]
        [HttpGet("paging")]
        [SwaggerOperation(Summary = "Retrieve paginated company")]
        public PagingDto<CompanyDto> RetrieveCompaniesPaging([FromQuery] PagingSearchInputBase input) => _companyService.RetrieveCompaniesPaging(input);

        [AppAuthorize(PermissionConstants.Organization.Company.View)]
        [HttpGet("{companyId:guid}")]
        [SwaggerOperation(Summary = "Retrieve company data by id")]
        public async Task<ObjectDto<CompanyDto>> RetrieveCompanyByIdAsync(Guid companyId) => await _companyService.RetrieveCompanyByIdAsync(companyId);

        [AppAuthorize(PermissionConstants.Organization.Company.Create)]
        [Mutation]
        [HttpPost("create")]
        [SwaggerOperation(Summary = "Create company")]
        public async Task<BaseDto> CreateCompanyAsync([FromBody] CompanyInput input) => await _companyService.CreateCompanyAsync(input);

        [AppAuthorize(PermissionConstants.Organization.Company.Update)]
        [Mutation]
        [HttpPut("update/{companyId:guid}")]
        [SwaggerOperation(Summary = "Update company")]
        public async Task<BaseDto> UpdateCompanyAsync(Guid companyId, [FromBody] CompanyInput input) => await _companyService.UpdateCompanyAsync(companyId, input);

        [AppAuthorize(PermissionConstants.Organization.Company.Delete)]
        [Mutation]
        [HttpDelete("delete/{companyId:guid}")]
        [SwaggerOperation(Summary = "Delete company")]
        public async Task<BaseDto> DeleteCompanyAsync(Guid companyId) => await _companyService.DeleteCompanyAsync(companyId);
    }
}
