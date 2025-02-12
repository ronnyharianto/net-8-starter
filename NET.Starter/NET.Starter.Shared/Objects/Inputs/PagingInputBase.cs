using NET.Starter.Shared.Objects.Inputs.Interfaces;

namespace NET.Starter.Shared.Objects.Inputs
{
    /// <summary>
    /// Represents a base class for paginated input requests.
    /// This class provides properties to manage pagination in API requests.
    /// </summary>
    public class PagingInputBase : IPagingInput
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}