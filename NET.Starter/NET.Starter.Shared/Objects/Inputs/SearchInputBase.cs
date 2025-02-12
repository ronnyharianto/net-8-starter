using NET.Starter.Shared.Objects.Inputs.Interfaces;

namespace NET.Starter.Shared.Objects.Inputs
{
    /// <summary>
    /// Represents a base class for search input requests.
    /// This class provides a common structure for search queries.
    /// </summary>
    public class SearchInputBase : ISearchInput
    {
        public string? SearchKey { get; set; }
    }
}