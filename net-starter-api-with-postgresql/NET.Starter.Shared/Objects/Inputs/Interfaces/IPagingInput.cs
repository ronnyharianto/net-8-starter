using System.ComponentModel.DataAnnotations;

namespace NET.Starter.Shared.Objects.Inputs.Interfaces
{
    /// <summary>
    /// Defines input properties required for paginated data requests.
    /// Classes implementing this interface should include pagination controls such as page number and page size.
    /// </summary>
    public interface IPagingInput
    {
        /// <summary>
        /// The page number to retrieve. Starts from 1.
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "Page must be greater than or equal to 1.")]
        int Page { get; set; }

        /// <summary>
        /// The number of items to return per page.
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "PageSize must be greater than or equal to 1.")]
        int PageSize { get; set; }
    }
}
