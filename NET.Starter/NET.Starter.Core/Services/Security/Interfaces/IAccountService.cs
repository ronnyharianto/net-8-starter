using NET.Starter.Core.Services.Security.Dtos;
using NET.Starter.Core.Services.Security.Inputs;
using NET.Starter.Shared.Objects.Dtos;

namespace NET.Starter.Core.Services.Security.Interfaces
{
    /// <summary>
    /// Provides methods for managing user account-related operations, including authentication.
    /// </summary>
    public interface IAccountService
    {
        /// <summary>
        /// Authenticates a user based on the provided login credentials.
        /// </summary>
        /// <param name="input">An object containing the user's login credentials.</param>
        /// <returns>
        /// A task representing the asynchronous operation. The task result contains an <see cref="ObjectDto{T}"/> 
        /// with the authentication result and associated data of type <see cref="LoginDto"/>.
        /// </returns>
        Task<ObjectDto<LoginDto>> LoginAsync(LoginInput input);
    }
}
