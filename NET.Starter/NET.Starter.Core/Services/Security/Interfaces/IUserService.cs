using NET.Starter.Core.Services.Security.Dtos;
using NET.Starter.Core.Services.Security.Inputs;
using NET.Starter.Shared.Objects.Dtos;
using NET.Starter.Shared.Objects.Inputs;

namespace NET.Starter.Core.Services.Security.Interfaces
{
    /// <summary>
    /// Provides methods for managing users in the system, including retrieving, creating, updating, and deleting users.
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Retrieves a paginated list of users based on the provided search and pagination parameters.
        /// </summary>
        /// <param name="input">The search and pagination input parameters.</param>
        /// <returns>
        /// A <see cref="PagingDto{T}"/> containing the paginated list of users of type <see cref="UserDto"/>.
        /// </returns>
        PagingDto<UserDto> RetrieveUsersPaging(PagingSearchInputBase input);

        /// <summary>
        /// Retrieves detailed information about a specific user by its unique identifier.
        /// </summary>
        /// <param name="userId">The unique identifier of the user to retrieve.</param>
        /// <returns>
        /// A task representing the asynchronous operation. The task result contains an <see cref="ObjectDto{T}"/> 
        /// with the user details of type <see cref="UserDto"/>.
        /// </returns>
        Task<ObjectDto<UserDto>> RetrieveUserByIdAsync(Guid userId);

        /// <summary>
        /// Creates a new user in the system.
        /// </summary>
        /// <param name="input">An object containing the details of the user to create.</param>
        /// <returns>
        /// A task representing the asynchronous operation. The task result contains a <see cref="BaseDto"/> 
        /// indicating the result of the operation.
        /// </returns>
        Task<BaseDto> CreateUserAsync(UserInput input);

        /// <summary>
        /// Updates an existing user in the system by its unique identifier.
        /// </summary>
        /// <param name="userId">The unique identifier of the user to update.</param>
        /// <param name="input">An object containing the updated details of the user.</param>
        /// <returns>
        /// A task representing the asynchronous operation. The task result contains a <see cref="BaseDto"/> 
        /// indicating the result of the operation.
        /// </returns>
        Task<BaseDto> UpdateUserAsync(Guid userId, UserInput input);

        /// <summary>
        /// Deletes an existing user in the system by its unique identifier.
        /// </summary>
        /// <param name="userId">The unique identifier of the user to delete.</param>
        /// <returns>
        /// A task representing the asynchronous operation. The task result contains a <see cref="BaseDto"/> 
        /// indicating the result of the operation.
        /// </returns>
        Task<BaseDto> DeleteUserAsync(Guid userId);

        /// <summary>
        /// Activates a user in the system by its unique identifier.
        /// </summary>
        /// <param name="userId">The unique identifier of the user to activate.</param>
        /// <returns>
        /// A task representing the asynchronous operation. The task result contains a <see cref="BaseDto"/>
        /// indicating the result of the operation.
        /// </returns>
        Task<BaseDto> ActivateUserAsync(Guid userId);

        /// <summary>
        /// Deactivates a user in the system by its unique identifier.
        /// </summary>
        /// <param name="userId">The unique identifier of the user to deactivate.</param>
        /// <returns>
        /// A task representing the asynchronous operation. The task result contains a <see cref="BaseDto"/>
        /// indicating the result of the operation.
        /// </returns>
        Task<BaseDto> DeactivateUserAsync(Guid userId);
    }
}
