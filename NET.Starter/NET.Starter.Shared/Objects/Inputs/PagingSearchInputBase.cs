using NET.Starter.Shared.Objects.Inputs.Interfaces;

namespace NET.Starter.Shared.Objects.Inputs
{
    /// <summary>
    /// Represents the base class for paginated search input.
    /// Combines pagination and search capabilities, enabling efficient
    /// data retrieval with filtering and pagination support.
    /// </summary>
    public class PagingSearchInputBase : IPagingInput, ISearchInput
    {
        public int Page { get; set; }

        public int PageSize { get; set; }

        public string? SearchKey { get; set; }
    }

}