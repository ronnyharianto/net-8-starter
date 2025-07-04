namespace NET.Starter.Core.Services.Security.Inputs
{
    public class UserInput
    {
        public string FullName { get; set; } = string.Empty;

        public string EmailAddress { get; set; } = string.Empty;

        public string PictureUrl { get; set; } = string.Empty;

        public IEnumerable<UserCompanyInput> UserCompanies { get; set; } = [];
    }

    public class UserCompanyInput
    {
        public Guid? UserCompanyId { get; set; }

        public Guid CompanyId { get; set; }

        public bool IsDefault { get; set; }

        public IEnumerable<Guid> RoleIds { get; set; } = [];
    }
}
