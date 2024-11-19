namespace NET.Starter.API.Shared.Objects.Inputs.Interfaces
{
    internal interface IPagingInput
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
