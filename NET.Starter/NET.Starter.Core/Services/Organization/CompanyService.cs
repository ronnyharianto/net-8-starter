using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NET.Starter.Core.Bases;
using NET.Starter.Core.Services.Organization.Dtos;
using NET.Starter.Core.Services.Organization.Inputs;
using NET.Starter.Core.Services.Security.Interfaces;
using NET.Starter.DataAccess.SqlServer;
using NET.Starter.DataAccess.SqlServer.Models.Organization;
using NET.Starter.Shared.Objects.Dtos;
using NET.Starter.Shared.Objects.Inputs;
using System.Net;

namespace NET.Starter.Core.Services.Organization
{
    internal class CompanyService(ApplicationDbContext dbContext, IMapper mapper, ILogger<CompanyService> logger)
        : BaseService<CompanyService>(dbContext, mapper, logger), ICompanyService
    {
        public async Task<ObjectDto<IEnumerable<CompanyDto>>> RetrieveCompaniesAsync()
        {
            _logger.LogInformation("Starting to retrieve all companies.");

            var companies = _dbContext.Companies.AsNoTracking()
                                        .OrderBy(d => d.CompanyCode)
                                        .Select(d => _mapper.Map<CompanyDto>(d));

            _logger.LogInformation("Successfully retrieved all companies.");

            return new(httpStatusCode: HttpStatusCode.OK)
            {
                Obj = await companies.ToListAsync()
            };
        }

        public PagingDto<CompanyDto> RetrieveCompaniesPaging(PagingSearchInputBase input)
        {
            _logger.LogInformation("Starting to retrieve paging of companies.");

            var retVal = new PagingDto<CompanyDto>();

            var searchKey = input.SearchKey?.Trim() ?? string.Empty;
            var searchPattern = $"%{searchKey}%";

            var companies = _dbContext.Companies.AsNoTracking()
                                                .Where(d => EF.Functions.Like(d.CompanyCode, searchPattern))
                                                .OrderByDescending(d => d.Modified ?? d.Created)
                                                .Select(d => _mapper.Map<CompanyDto>(d));

            retVal.ApplyPagination(input.Page, input.PageSize, companies);

            _logger.LogInformation("Successfully retrieved paging of companies.");

            return retVal;
        }

        public async Task<ObjectDto<CompanyDto>> RetrieveCompanyByIdAsync(Guid companyId)
        {
            _logger.LogInformation("Starting to retrieve company by id: {CompanyId}.", companyId);

            var company = await _dbContext.Companies.AsNoTracking().FirstOrDefaultAsync(d => d.Id == companyId);
            if (company == null)
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
                _logger.LogError("Failed to create company. Reason : {ValidationMessage}", validationMessage);

                return new(validationMessage, HttpStatusCode.InternalServerError);
            }

            var company = _mapper.Map<Company>(input, opts => opts.Items["IsCreate"] = true);

            await _dbContext.Companies.AddAsync(company);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("Successfully created company.");

            return new("Company data is successfully created", HttpStatusCode.OK);
        }

        public async Task<BaseDto> UpdateCompanyAsync(Guid companyId, CompanyInput input)
        {
            _logger.LogInformation("Starting to update company by id: {CompanyId}.", companyId);

            var (isValid, validationMessage) = await ValidateCompanyInput(input, companyId);
            if (!isValid)
            {
                _logger.LogError("Failed to update company. Reason : {ValidationMessage}", validationMessage);

                return new(validationMessage, HttpStatusCode.InternalServerError);
            }

            var company = await _dbContext.Companies.FirstOrDefaultAsync(d => d.Id == companyId);
            if (company == null)
            {
                _logger.LogError("Company data is not found for id: {CompanyId}.", companyId);

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
            if (company == null)
            {
                _logger.LogError("Company data is not found for id: {CompanyId}.", companyId);

                return new("Company data is not found", HttpStatusCode.NotFound);
            }
            
            company.RowStatus = 1;

            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("Successfully deleted company by id: {CompanyId}.", companyId);

            return new("Company data is successfully deleted", HttpStatusCode.OK);
        }

        /// <summary>
        /// Validates the company input for duplicate company codes and required permissions.
        /// </summary>
        /// <param name="input">The company input to validate.</param>
        /// <returns>
        /// A tuple containing a boolean indicating validity and a validation message.
        /// </returns>
        private async Task<(bool isValid, string validationMessage)> ValidateCompanyInput(CompanyInput input, Guid? companyId = null)
        {
            if (string.IsNullOrWhiteSpace(input.CompanyName))
                return (false, "Company name is required.");

            if (!companyId.HasValue)
            {
                if (string.IsNullOrWhiteSpace(input.CompanyCode))
                    return (false, "Company code is required.");
            }
            else
            {
                var dataDuplicateCompany = await _dbContext.Companies.FirstOrDefaultAsync(d => d.CompanyCode == input.CompanyCode && d.Id != companyId);
                if (dataDuplicateCompany != null)
                    return (false, "Company already exists.");
            }

            return (true, string.Empty);
        }
    }
}
