using NET.Starter.Core.Services.Organization.Dtos;

namespace NET.Starter.Core.Services.Security.Dtos
{
    public class UserDto
    {
        public Guid UserId { get; set; }

        public required string Username { get; set; }

        public required string EmailAddress { get; set; }

        public required string Fullname { get; set; }

        public IEnumerable<UserCompanyDto> UserCompanies { get; set; } = [];
    }

    public class UserCompanyDto
    {
        public Guid UserCompanyId { get; set; }

        public Guid CompanyId { get; set; }

        public bool IsDefault { get; set; }

        public CompanyDto Company { get; set; } = null!;

        public IEnumerable<RoleDto> Roles { get; set; } = [];
    }
}
