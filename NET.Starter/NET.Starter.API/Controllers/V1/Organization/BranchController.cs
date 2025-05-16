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
    public class BranchController(IBranchService branchService) : BaseController
    {
        private readonly IBranchService _branchService = branchService;

        [AppAuthorize(PermissionConstants.Organization.Branch.View)]
        [HttpGet("all")]
        [SwaggerOperation(Summary = "Retrieve all branch")]
        public async Task<ObjectDto<IEnumerable<BranchDto>>> RetrieveBranchesAsync() => await _branchService.RetrieveBranchesAsync();

        [AppAuthorize(PermissionConstants.Organization.Branch.View)]
        [HttpGet("paging")]
        [SwaggerOperation(Summary = "Retrieve paginated branch")]
        public PagingDto<BranchDto> RetrieveBranchesPaging([FromQuery] PagingSearchInputBase input) => _branchService.RetrieveBranchesPaging(input);

        [AppAuthorize(PermissionConstants.Organization.Branch.View)]
        [HttpGet("{branchId:guid}")]
        [SwaggerOperation(Summary = "Retrieve branch data by id")]
        public async Task<ObjectDto<BranchDto>> RetrieveBranchByIdAsync(Guid branchId) => await _branchService.RetrieveBranchByIdAsync(branchId);

        [AppAuthorize(PermissionConstants.Organization.Branch.Create)]
        [Mutation]
        [HttpPost("create")]
        [SwaggerOperation(Summary = "Create branch")]
        public async Task<BaseDto> CreateBranchAsync([FromBody] BranchInput input) => await _branchService.CreateBranchAsync(input);

        [AppAuthorize(PermissionConstants.Organization.Branch.Update)]
        [Mutation]
        [HttpPut("update/{branchId:guid}")]
        [SwaggerOperation(Summary = "Update branch")]
        public async Task<BaseDto> UpdateBranchAsync(Guid branchId, [FromBody] BranchInput input) => await _branchService.UpdateBranchAsync(branchId, input);

        [AppAuthorize(PermissionConstants.Organization.Branch.Delete)]
        [Mutation]
        [HttpDelete("delete/{branchId:guid}")]
        [SwaggerOperation(Summary = "Delete branch")]
        public async Task<BaseDto> DeleteBranchAsync(Guid branchId) => await _branchService.DeleteBranchAsync(branchId);
    }
}
