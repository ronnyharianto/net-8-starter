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
    internal class BranchService(ApplicationDbContext dbContext, IMapper mapper, ILogger<BranchService> logger)
        : BaseService<BranchService>(dbContext, mapper, logger), IBranchService
    {
        public async Task<ObjectDto<IEnumerable<BranchDto>>> RetrieveBranchesAsync()
        {
            _logger.LogInformation("Starting to retrieve all branches.");

            var branches = _dbContext.Branches.AsNoTracking()
                                              .OrderBy(d => d.BranchCode)
                                              .Select(d => _mapper.Map<BranchDto>(d));

            _logger.LogInformation("Successfully retrieved all branches.");

            return new(httpStatusCode: HttpStatusCode.OK)
            {
                Obj = await branches.ToListAsync()
            };
        }

        public PagingDto<BranchDto> RetrieveBranchesPaging(PagingSearchInputBase input)
        {
            _logger.LogInformation("Starting to retrieve paging of branches.");

            var retVal = new PagingDto<BranchDto>();

            var searchKey = input.SearchKey?.Trim() ?? string.Empty;
            var searchPattern = $"%{searchKey}%";

            var branches = _dbContext.Branches.AsNoTracking()
                                              .Where(d => EF.Functions.Like(d.BranchCode, searchPattern))
                                              .OrderByDescending(d => d.Modified ?? d.Created)
                                              .Select(d => _mapper.Map<BranchDto>(d));

            retVal.ApplyPagination(input.Page, input.PageSize, branches);

            _logger.LogInformation("Successfully retrieved paging of branches.");

            return retVal;
        }

        public async Task<ObjectDto<BranchDto>> RetrieveBranchByIdAsync(Guid branchId)
        {
            _logger.LogInformation("Starting to retrieve branch by id: {BranchId}.", branchId);

            var branch = await _dbContext.Branches.Include(b => b.Company).AsNoTracking().FirstOrDefaultAsync(d => d.Id == branchId);
            if (branch == null)
            {
                _logger.LogError("Branch data is not found for id: {BranchId}.", branchId);

                return new("Branch data is not found", HttpStatusCode.NotFound);
            }
            
            _logger.LogInformation("Successfully retrieved branch by id: {BranchId}.", branchId);

            return new(httpStatusCode: HttpStatusCode.OK)
            {
                Obj = _mapper.Map<BranchDto>(branch)
            };
        }

        public async Task<BaseDto> CreateBranchAsync(BranchInput input)
        {
            _logger.LogInformation("Starting to create branch.");

            var (isValid, validationMessage) = await ValidateBranchInput(input);
            if (!isValid)
            {
                _logger.LogError("Failed to create branch. Reason : {ValidationMessage}", validationMessage);

                return new(validationMessage, HttpStatusCode.InternalServerError);
            }

            var branch = _mapper.Map<Branch>(input, opts => opts.Items["IsCreate"] = true);

            await _dbContext.Branches.AddAsync(branch);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("Successfully created branch.");

            return new("Branch data is successfully created", HttpStatusCode.OK);
        }

        public async Task<BaseDto> UpdateBranchAsync(Guid branchId, BranchInput input)
        {
            _logger.LogInformation("Starting to update branch by id: {BranchId}.", branchId);

            var (isValid, validationMessage) = await ValidateBranchInput(input, branchId);
            if (!isValid)
            {
                _logger.LogError("Failed to update branch. Reason : {ValidationMessage}", validationMessage);

                return new(validationMessage, HttpStatusCode.InternalServerError);
            }

            var branch = await _dbContext.Branches.FirstOrDefaultAsync(d => d.Id == branchId);
            if (branch == null)
            {
                _logger.LogError("Branch data is not found for id: {BranchId}.", branchId);

                return new("Branch data is not found", HttpStatusCode.NotFound);
            }

            _mapper.Map(input, branch);

            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("Successfully updated branch by id: {BranchId}.", branchId);

            return new("Branch data is successfully updated", HttpStatusCode.OK);
        }

        public async Task<BaseDto> DeleteBranchAsync(Guid branchId)
        {
            _logger.LogInformation("Starting to delete branch by id: {BranchId}.", branchId);

            var branch = await _dbContext.Branches.FirstOrDefaultAsync(d => d.Id == branchId);
            if (branch == null)
            {
                _logger.LogError("Branch data is not found for id: {BranchId}.", branchId);

                return new("Branch data is not found", HttpStatusCode.NotFound);
            }
            
            branch.RowStatus = 1;

            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("Successfully deleted branch by id: {BranchId}.", branchId);

            return new("Branch data is successfully deleted", HttpStatusCode.OK);
        }

        /// <summary>
        /// Validates the branch input for duplicate branch codes and required permissions.
        /// </summary>
        /// <param name="input">The branch input to validate.</param>
        /// <returns>
        /// A tuple containing a boolean indicating validity and a validation message.
        /// </returns>
        private async Task<(bool isValid, string validationMessage)> ValidateBranchInput(BranchInput input, Guid? branchId = null)
        {
            if (string.IsNullOrWhiteSpace(input.BranchName))
                return (false, "Branch name is required.");

            if (!branchId.HasValue)
            {
                if (string.IsNullOrWhiteSpace(input.BranchCode))
                    return (false, "Branch code is required.");
            }
            else
            {
                var dataDuplicateBranch = await _dbContext.Branches.FirstOrDefaultAsync(d => d.BranchCode == input.BranchCode && d.Id != branchId);
                if (dataDuplicateBranch != null)
                    return (false, "Branch already exists.");
            }

            return (true, string.Empty);
        }
    }
}
