using NET.Starter.Shared.Objects.Inputs.Interfaces;

namespace NET.Starter.Shared.Objects.Inputs
{
    public class PagingInputBase : IPagingInput
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
