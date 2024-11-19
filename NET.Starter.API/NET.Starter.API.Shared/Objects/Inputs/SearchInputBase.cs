using NET.Starter.API.Shared.Objects.Inputs.Interfaces;

namespace NET.Starter.API.Shared.Objects.Inputs
{
    public class SearchInputBase : ISearchInput
    {
        public string? SearchKey { get; set; }
    }
}
