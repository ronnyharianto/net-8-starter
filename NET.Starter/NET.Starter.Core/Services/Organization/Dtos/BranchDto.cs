namespace NET.Starter.Core.Services.Organization.Dtos
{
    public class BranchDto
    {
        public Guid BranchId { get; set; }

        public string BranchCode { get; set; } = string.Empty;

        public string BranchName { get; set; } = string.Empty;

        public string BranchTimeZone { get; set; } = string.Empty;

        public CompanyDto Company { get; set; } = null!;
    }
}
