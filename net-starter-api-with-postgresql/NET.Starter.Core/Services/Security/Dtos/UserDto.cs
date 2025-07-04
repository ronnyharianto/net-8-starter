using NET.Starter.Core.Services.Organization.Dtos;

namespace NET.Starter.Core.Services.Security.Dtos
{
    public class UserDto
    {
        public Guid UserId { get; set; }

        public string EmailAddress { get; set; } = string.Empty;

        public string FullName { get; set; } = string.Empty;

        public string PictureUrl { get; set; } = string.Empty;

        public bool IsActive { get; set; }

        public IEnumerable<UserCompanyDto> UserCompanies { get; set; } = [];
    }

    public class UserCompanyDto
    {
        public Guid UserCompanyId { get; set; }

        public Guid CompanyId { get; set; }

        public bool IsDefault { get; set; }

        public CompanyDto Company { get; set; } = null!;

        public IEnumerable<UserCompanyRoleDto> UserCompanyRoles { get; set; } = [];
    }

    public class UserCompanyRoleDto 
    {
        public Guid UserCompanyRoleId { get; set; }

        public Guid RoleId { get; set; }

        public RoleDto Role { get; set; } = null!;
    }
}
