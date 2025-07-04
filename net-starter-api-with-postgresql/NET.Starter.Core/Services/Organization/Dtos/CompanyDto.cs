namespace NET.Starter.Core.Services.Organization.Dtos
{
    public class CompanyDto
    {
        public Guid CompanyId { get; set; }

        public string Code { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Website { get; set; } = string.Empty;
    }
}
