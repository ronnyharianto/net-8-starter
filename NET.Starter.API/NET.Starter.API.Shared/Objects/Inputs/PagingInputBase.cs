using NET.Starter.API.Shared.Objects.Inputs.Interfaces;

namespace NET.Starter.API.Shared.Objects.Inputs
{
    internal class PagingInputBase : IPagingInput
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
