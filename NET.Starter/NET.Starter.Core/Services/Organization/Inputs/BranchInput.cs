namespace NET.Starter.Core.Services.Organization.Inputs
{
    public class BranchInput
    {
        public Guid CompanyId { get; set; }

        public required string BranchCode { get; set; }

        public required string BranchName { get; set; }

        public required string BranchTimeZone { get; set; }
    }
}
