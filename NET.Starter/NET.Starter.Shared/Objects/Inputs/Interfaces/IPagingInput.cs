using System.ComponentModel.DataAnnotations;

namespace NET.Starter.Shared.Objects.Inputs.Interfaces
{
    /// <summary>
    /// Represents a paging input interface for paginated requests.
    /// Any class that implements this interface will require page and page size properties.
    /// </summary>
    public interface IPagingInput
    {
        /// <summary>
        /// Gets or sets the current page number.
        /// The value should be greater than or equal to 1.
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "Page must be greater than or equal to 1.")]
        public int Page { get; set; }

        /// <summary>
        /// Gets or sets the number of items per page.
        /// The value should be greater than or equal to 1.
        /// A common default value is 10 or 20.
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "PageSize must be greater than or equal to 1.")]
        public int PageSize { get; set; }
    }
}