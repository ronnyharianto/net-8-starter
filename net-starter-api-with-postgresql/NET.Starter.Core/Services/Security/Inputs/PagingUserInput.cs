using NET.Starter.Shared.Objects.Inputs;

namespace NET.Starter.Core.Services.Security.Inputs
{
    public class PagingUserInput : PagingSearchInputBase
    {
        public Guid? CompanyId { get; set; }
    }
}
