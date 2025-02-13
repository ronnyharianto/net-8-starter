using NET.Starter.Core.Services.Security.Dtos;
using NET.Starter.Core.Services.Security.Inputs;
using NET.Starter.Shared.Objects.Dtos;
using NET.Starter.Shared.Objects.Inputs;

namespace NET.Starter.Core.Services.Security.Interfaces
{
    /// <summary>
    /// Provides methods for managing roles in the system, including retrieving, creating, updating, and deleting roles.
    /// </summary>
    public interface IRoleService
    {
        /// <summary>
        /// Retrieves all roles available in the system.
        /// </summary>
        /// <returns>
        /// A task representing the asynchronous operation. The task result contains an <see cref="ObjectDto{T}"/> 
        /// with a collection of <see cref="RoleDto"/> representing the roles.
        /// </returns>
        Task<ObjectDto<IEnumerable<RoleDto>>> RetrieveRolesAsync();

        /// <summary>
        /// Retrieves a paginated list of roles based on the provided search and pagination parameters.
        /// </summary>
        /// <param name="input">The search and pagination input parameters.</param>
        /// <returns>
        /// A <see cref="PagingDto{T}"/> containing the paginated list of roles of type <see cref="RoleDto"/>.
        /// </returns>
        PagingDto<RoleDto> RetrieveRolesPaging(PagingSearchInputBase input);

        /// <summary>
        /// Retrieves detailed information about a specific role by its unique identifier.
        /// </summary>
        /// <param name="roleId">The unique identifier of the role to retrieve.</param>
        /// <returns>
        /// A task representing the asynchronous operation. The task result contains an <see cref="ObjectDto{T}"/> 
        /// with the role details of type <see cref="RoleDto"/>.
        /// </returns>
        Task<ObjectDto<RoleDto>> RetrieveRoleByIdAsync(Guid roleId);

        /// <summary>
        /// Creates a new role in the system.
        /// </summary>
        /// <param name="input">An object containing the details of the role to create.</param>
        /// <returns>
        /// A task representing the asynchronous operation. The task result contains a <see cref="BaseDto"/> 
        /// indicating the result of the operation.
        /// </returns>
        Task<BaseDto> CreateRoleAsync(RoleInput input);

        /// <summary>
        /// Updates an existing role in the system by its unique identifier.
        /// </summary>
        /// <param name="roleId">The unique identifier of the role to update.</param>
        /// <param name="input">An object containing the updated details of the role.</param>
        /// <returns>
        /// A task representing the asynchronous operation. The task result contains a <see cref="BaseDto"/> 
        /// indicating the result of the operation.
        /// </returns>
        Task<BaseDto> UpdateRoleAsync(Guid roleId, RoleInput input);

        /// <summary>
        /// Deletes an existing role in the system by its unique identifier.
        /// </summary>
        /// <param name="roleId">The unique identifier of the role to delete.</param>
        /// <returns>
        /// A task representing the asynchronous operation. The task result contains a <see cref="BaseDto"/> 
        /// indicating the result of the operation.
        /// </returns>
        Task<BaseDto> DeleteRoleAsync(Guid roleId);
    }
}
