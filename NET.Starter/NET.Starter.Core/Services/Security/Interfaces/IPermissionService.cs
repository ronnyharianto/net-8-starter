using NET.Starter.Core.Services.Security.Dtos;
using NET.Starter.Shared.Objects.Dtos;

namespace NET.Starter.Core.Services.Security.Interfaces
{
    /// <summary>
    /// Provides methods for managing and retrieving permissions in the system.
    /// </summary>
    public interface IPermissionService
    {
        /// <summary>
        /// Retrieves all permissions available in the system.
        /// </summary>
        /// <returns>
        /// A task representing the asynchronous operation. The task result contains an <see cref="ObjectDto{T}"/> 
        /// with a collection of <see cref="PermissionDto"/> representing the permissions.
        /// </returns>
        Task<ObjectDto<IEnumerable<PermissionDto>>> RetrievePermissionsAsync();
    }
}
