using NET.Starter.Shared.Objects.Inputs.Interfaces;

namespace NET.Starter.Shared.Objects.Inputs
{
    /// <summary>
    /// Represents a base input model that combines pagination and search functionality.
    /// Useful for queries that require both paginated results and keyword-based filtering.
    /// </summary>
    /// <remarks>
    /// This class can be inherited by specific input models used in data retrieval endpoints.
    /// </remarks>
    public class PagingSearchInputBase : IPagingInput, ISearchInput
    {
        public int Page { get; set; }

        public int PageSize { get; set; }

        public string? SearchKey { get; set; }
    }
}