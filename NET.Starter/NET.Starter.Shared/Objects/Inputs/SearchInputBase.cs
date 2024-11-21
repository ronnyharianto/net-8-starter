using NET.Starter.Shared.Objects.Inputs.Interfaces;

namespace NET.Starter.Shared.Objects.Inputs
{
    public class SearchInputBase : ISearchInput
    {
        public string? SearchKey { get; set; }
    }
}
