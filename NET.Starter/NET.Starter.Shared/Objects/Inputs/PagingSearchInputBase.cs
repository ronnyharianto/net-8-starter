using NET.Starter.Shared.Objects.Inputs.Interfaces;

namespace NET.Starter.Shared.Objects.Inputs
{
    public class PagingSearchInputBase : IPagingInput, ISearchInput
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string? SearchKey { get; set; }
    }
}
