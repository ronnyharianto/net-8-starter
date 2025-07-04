using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NET.Starter.Core.Services.Organization.Dtos;
using NET.Starter.Core.Services.Organization.Inputs;
using NET.Starter.Core.Services.Organization.Interfaces;
using NET.Starter.DataAccess;
using NET.Starter.DataAccess.Models.Organization;
using NET.Starter.Shared.Objects.Dtos;
using NET.Starter.Shared.Objects.Inputs;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace NET.Starter.Core.Services.Organization
{
    internal class CompanyService(ApplicationDbContext _dbContext, IMapper _mapper, ILogger<CompanyService> _logger) : ICompanyService
    {
        public async Task<ObjectDto<IEnumerable<CompanyDto>>> RetrieveCompaniesAsync()
        {
            _logger.LogInformation("Starting to retrieve all companies.");

            var companies = _dbContext.Companies.AsNoTracking()
                                                .OrderBy(d => d.Name)
                                                .Select(d => _mapper.Map<CompanyDto>(d));

            var result = await companies.ToListAsync();

            _logger.LogInformation("Successfully retrieved {Count} companies.", result.Count);
            return new(httpStatusCode: HttpStatusCode.OK)
            {
                Obj = result
            };
        }

        public async Task<PagingDto<CompanyDto>> RetrieveCompaniesPagingAsync(PagingSearchInputBase input)
        {
            _logger.LogInformation("Starting to retrieve paging of companies. Page: {Page}, Size: {PageSize}, SearchKey: {SearchKey}",
                input.Page, input.PageSize, input.SearchKey);

            var retVal = new PagingDto<CompanyDto>();

            var searchKey = input.SearchKey?.Trim() ?? string.Empty;
            var searchPattern = $"%{searchKey}%";

            var companies = _dbContext.Companies.AsNoTracking()
                                                .Where(d => 
                                                    EF.Functions.ILike(d.Code, searchPattern)
                                                    || EF.Functions.ILike(d.Name, searchPattern)
                                                    || EF.Functions.ILike(d.Address, searchPattern)
                                                )
                                                .OrderByDescending(d => d.Modified ?? d.Created)
                                                .ThenBy(d => d.Code)
                                                .Select(d => _mapper.Map<CompanyDto>(d));

            await retVal.ApplyPagination(input.Page, input.PageSize, companies);

            _logger.LogInformation("Successfully retrieved {Count} companies on page {Page}.", retVal.PageRecordCount, input.Page);
            return retVal;
        }

        public async Task<ObjectDto<CompanyDto>> RetrieveCompanyByIdAsync(Guid companyId)
        {
            _logger.LogInformation("Starting to retrieve company by id: {CompanyId}.", companyId);

            var company = await _dbContext.Companies.AsNoTracking().FirstOrDefaultAsync(d => d.Id == companyId);
            if (company is null)
            {
                _logger.LogError("Company data is not found for id: {CompanyId}.", companyId);
                return new("Company data is not found", HttpStatusCode.NotFound);
            }

            _logger.LogInformation("Successfully retrieved company by id: {CompanyId}.", companyId);
            return new(httpStatusCode: HttpStatusCode.OK)
            {
                Obj = _mapper.Map<CompanyDto>(company)
            };
        }

        public async Task<BaseDto> CreateCompanyAsync(CompanyInput input)
        {
            _logger.LogInformation("Starting to create company.");

            var (isValid, validationMessage) = await ValidateCompanyInput(input);
            if (!isValid)
            {
                _logger.LogWarning("Validation failed during company creation. Reason: {ValidationMessage}", validationMessage);
                return new(validationMessage, HttpStatusCode.InternalServerError);
            }

            var company = _mapper.Map<Company>(input, opts => opts.Items["IsCreate"] = true);

            await _dbContext.Companies.AddAsync(company);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("Company entity created: {@Company}", company);
            return new("Company data is successfully created", HttpStatusCode.OK);
        }

        public async Task<BaseDto> UpdateCompanyAsync(Guid companyId, CompanyInput input)
        {
            _logger.LogInformation("Starting to update company by id: {CompanyId}.", companyId);

            var (isValid, validationMessage) = await ValidateCompanyInput(input, companyId);
            if (!isValid)
            {
                _logger.LogWarning("Validation failed during company update. Reason: {ValidationMessage}", validationMessage);
                return new(validationMessage, HttpStatusCode.InternalServerError);
            }

            var company = await _dbContext.Companies.FirstOrDefaultAsync(d => d.Id == companyId);
            if (company is null)
            {
                _logger.LogWarning("Update failed. Company with ID: {CompanyId} not found.", companyId);
                return new("Company data is not found", HttpStatusCode.NotFound);
            }

            _mapper.Map(input, company);

            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("Successfully updated company by id: {CompanyId}.", companyId);
            return new("Company data is successfully updated", HttpStatusCode.OK);
        }

        public async Task<BaseDto> DeleteCompanyAsync(Guid companyId)
        {
            _logger.LogInformation("Starting to delete company by id: {CompanyId}.", companyId);

            var company = await _dbContext.Companies.FirstOrDefaultAsync(d => d.Id == companyId);
            if (company is null)
            {
                _logger.LogError("Company data is not found for id: {CompanyId}.", companyId);
                return new("Company data is not found", HttpStatusCode.NotFound);
            }

            company.RowStatus = 1;

            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("Successfully deleted company by id: {CompanyId}.", companyId);

            return new("Company data is successfully deleted", HttpStatusCode.OK);
        }

        private async Task<(bool isValid, string validationMessage)> ValidateCompanyInput(CompanyInput input, Guid? companyId = null)
        {
            if (string.IsNullOrWhiteSpace(input.Name))
                return (false, "Name is required.");

            if (string.IsNullOrWhiteSpace(input.Code))
                return (false, "Code is required.");

            if (!string.IsNullOrWhiteSpace(input.Email) && !new EmailAddressAttribute().IsValid(input.Email))
                return (false, "Email is invalid.");

            var dataDuplicateCompany = await _dbContext.Companies.FirstOrDefaultAsync(d => d.Code == input.Code && d.Id != companyId);
            if (dataDuplicateCompany != null)
            {
                _logger.LogWarning("Company code {Code} already exists (ID: {Id})", input.Code, dataDuplicateCompany.Id);
                return (false, "Company already exists.");
            }

            return (true, string.Empty);
        }
    }
}