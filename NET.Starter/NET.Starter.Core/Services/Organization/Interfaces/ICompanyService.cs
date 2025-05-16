using NET.Starter.Core.Services.Organization.Dtos;
using NET.Starter.Core.Services.Organization.Inputs;
using NET.Starter.Shared.Objects.Dtos;
using NET.Starter.Shared.Objects.Inputs;

namespace NET.Starter.Core.Services.Security.Interfaces
{
    /// <summary>
    /// Provides methods for managing companies in the system, including retrieving, creating, updating, and deleting companies.
    /// </summary>
    public interface ICompanyService
    {
        /// <summary>
        /// Retrieves all companies available in the system.
        /// </summary>
        /// <returns>
        /// A task representing the asynchronous operation. The task result contains an <see cref="ObjectDto{T}"/> 
        /// with a collection of <see cref="CompanyDto"/> representing the companies.
        /// </returns>
        Task<ObjectDto<IEnumerable<CompanyDto>>> RetrieveCompaniesAsync();

        /// <summary>
        /// Retrieves a paginated list of companies based on the provided search and pagination parameters.
        /// </summary>
        /// <param name="input">The search and pagination input parameters.</param>
        /// <returns>
        /// A <see cref="PagingDto{T}"/> containing the paginated list of companies of type <see cref="CompanyDto"/>.
        /// </returns>
        PagingDto<CompanyDto> RetrieveCompaniesPaging(PagingSearchInputBase input);

        /// <summary>
        /// Retrieves detailed information about a specific company by its unique identifier.
        /// </summary>
        /// <param name="companyId">The unique identifier of the company to retrieve.</param>
        /// <returns>
        /// A task representing the asynchronous operation. The task result contains an <see cref="ObjectDto{T}"/> 
        /// with the company details of type <see cref="CompanyDto"/>.
        /// </returns>
        Task<ObjectDto<CompanyDto>> RetrieveCompanyByIdAsync(Guid companyId);

        /// <summary>
        /// Creates a new company in the system.
        /// </summary>
        /// <param name="input">An object containing the details of the company to create.</param>
        /// <returns>
        /// A task representing the asynchronous operation. The task result contains a <see cref="BaseDto"/> 
        /// indicating the result of the operation.
        /// </returns>
        Task<BaseDto> CreateCompanyAsync(CompanyInput input);

        /// <summary>
        /// Updates an existing company in the system by its unique identifier.
        /// </summary>
        /// <param name="companyId">The unique identifier of the company to update.</param>
        /// <param name="input">An object containing the updated details of the company.</param>
        /// <returns>
        /// A task representing the asynchronous operation. The task result contains a <see cref="BaseDto"/> 
        /// indicating the result of the operation.
        /// </returns>
        Task<BaseDto> UpdateCompanyAsync(Guid companyId, CompanyInput input);

        /// <summary>
        /// Deletes an existing company in the system by its unique identifier.
        /// </summary>
        /// <param name="companyId">The unique identifier of the company to delete.</param>
        /// <returns>
        /// A task representing the asynchronous operation. The task result contains a <see cref="BaseDto"/> 
        /// indicating the result of the operation.
        /// </returns>
        Task<BaseDto> DeleteCompanyAsync(Guid companyId);
    }
}
