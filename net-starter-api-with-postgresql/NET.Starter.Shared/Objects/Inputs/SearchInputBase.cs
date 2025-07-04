using NET.Starter.Shared.Objects.Inputs.Interfaces;

namespace NET.Starter.Shared.Objects.Inputs
{
    /// <summary>
    /// Base model for inputs that support keyword-based search.
    /// </summary>
    public class SearchInputBase : ISearchInput
    {
        public string? SearchKey { get; set; }
    }
}