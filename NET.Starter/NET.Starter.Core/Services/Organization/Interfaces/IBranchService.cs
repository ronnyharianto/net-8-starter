using NET.Starter.Core.Services.Organization.Dtos;
using NET.Starter.Core.Services.Organization.Inputs;
using NET.Starter.Shared.Objects.Dtos;
using NET.Starter.Shared.Objects.Inputs;

namespace NET.Starter.Core.Services.Security.Interfaces
{
    /// <summary>
    /// Provides methods for managing branches in the system, including retrieving, creating, updating, and deleting branches.
    /// </summary>
    public interface IBranchService
    {
        /// <summary>
        /// Retrieves all branches available in the system.
        /// </summary>
        /// <returns>
        /// A task representing the asynchronous operation. The task result contains an <see cref="ObjectDto{T}"/> 
        /// with a collection of <see cref="BranchDto"/> representing the branches.
        /// </returns>
        Task<ObjectDto<IEnumerable<BranchDto>>> RetrieveBranchesAsync();

        /// <summary>
        /// Retrieves a paginated list of branches based on the provided search and pagination parameters.
        /// </summary>
        /// <param name="input">The search and pagination input parameters.</param>
        /// <returns>
        /// A <see cref="PagingDto{T}"/> containing the paginated list of branches of type <see cref="BranchDto"/>.
        /// </returns>
        PagingDto<BranchDto> RetrieveBranchesPaging(PagingSearchInputBase input);

        /// <summary>
        /// Retrieves detailed information about a specific branch by its unique identifier.
        /// </summary>
        /// <param name="branchId">The unique identifier of the branch to retrieve.</param>
        /// <returns>
        /// A task representing the asynchronous operation. The task result contains an <see cref="ObjectDto{T}"/> 
        /// with the branch details of type <see cref="BranchDto"/>.
        /// </returns>
        Task<ObjectDto<BranchDto>> RetrieveBranchByIdAsync(Guid branchId);

        /// <summary>
        /// Creates a new branch in the system.
        /// </summary>
        /// <param name="input">An object containing the details of the branch to create.</param>
        /// <returns>
        /// A task representing the asynchronous operation. The task result contains a <see cref="BaseDto"/> 
        /// indicating the result of the operation.
        /// </returns>
        Task<BaseDto> CreateBranchAsync(BranchInput input);

        /// <summary>
        /// Updates an existing branch in the system by its unique identifier.
        /// </summary>
        /// <param name="branchId">The unique identifier of the branch to update.</param>
        /// <param name="input">An object containing the updated details of the branch.</param>
        /// <returns>
        /// A task representing the asynchronous operation. The task result contains a <see cref="BaseDto"/> 
        /// indicating the result of the operation.
        /// </returns>
        Task<BaseDto> UpdateBranchAsync(Guid branchId, BranchInput input);

        /// <summary>
        /// Deletes an existing branch in the system by its unique identifier.
        /// </summary>
        /// <param name="branchId">The unique identifier of the branch to delete.</param>
        /// <returns>
        /// A task representing the asynchronous operation. The task result contains a <see cref="BaseDto"/> 
        /// indicating the result of the operation.
        /// </returns>
        Task<BaseDto> DeleteBranchAsync(Guid branchId);
    }
}
