using NET.Starter.Shared.Objects.Inputs.Interfaces;

namespace NET.Starter.Shared.Objects.Inputs
{
    /// <summary>
    /// Represents a base class for paginated search input requests.
    /// This class combines pagination and search functionality,
    /// allowing filtered and paginated data retrieval.
    /// </summary>
    public class PagingSearchInputBase : IPagingInput, ISearchInput
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string? SearchKey { get; set; }
    }
}