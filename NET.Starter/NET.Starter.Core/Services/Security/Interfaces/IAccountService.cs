using NET.Starter.Core.Services.Organization.Dtos;
using NET.Starter.Core.Services.Security.CustomModels;
using NET.Starter.Core.Services.Security.Inputs;
using NET.Starter.Shared.Objects.Dtos;

namespace NET.Starter.Core.Services.Security.Interfaces
{
    /// <summary>
    /// Defines operations related to user authentication and company switching.
    /// </summary>
    public interface IAccountService
    {
        /// <summary>
        /// Authenticates a user using the provided login credentials.
        /// </summary>
        /// <param name="input">The login credentials.</param>
        /// <returns>
        /// An <see cref="ObjectDto{T}"/> containing the authentication result and associated <see cref="TokenResult"/> data.
        /// </returns>
        Task<ObjectDto<TokenResult>> LoginAsync(LoginInput input);

        /// <summary>
        /// Retrieves the list of companies associated with the currently authenticated user.
        /// </summary>
        /// <returns>
        /// An <see cref="ObjectDto{T}"/> containing a list of <see cref="CompanyDto"/> instances linked to the authenticated user.
        /// </returns>
        Task<ObjectDto<IEnumerable<CompanyDto>>> RetrieveMyCompaniesAsync();

        /// <summary>
        /// Refreshes the authentication token for the currently authenticated user.
        /// </summary>
        /// <param name="companyId">The Id of the company to refresh the token for.</param>
        /// <returns>
        /// An <see cref="ObjectDto{T}"/> containing the updated authentication result and associated <see cref="TokenResult"/> data.
        /// </returns>
        Task<ObjectDto<TokenResult>> RefreshTokenAsync(Guid? companyId = null);
    }
}
