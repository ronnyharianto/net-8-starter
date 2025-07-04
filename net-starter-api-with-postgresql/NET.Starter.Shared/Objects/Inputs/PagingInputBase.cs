using NET.Starter.Shared.Objects.Inputs.Interfaces;

namespace NET.Starter.Shared.Objects.Inputs
{
    /// <summary>
    /// A base input model for paginated requests.
    /// Provides standard pagination properties such as page number and page size.
    /// </summary>
    public class PagingInputBase : IPagingInput
    {
        public int Page { get; set; }

        public int PageSize { get; set; }
    }
}