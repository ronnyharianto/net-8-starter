namespace NET.Starter.Core.Services.Organization.Dtos
{
    public class CompanyDto
    {
        public Guid CompanyId { get; set; }

        public string CompanyCode { get; set; } = string.Empty;

        public string CompanyName { get; set; } = string.Empty;
    }
}
