namespace NET.Starter.Core.Services.Security.Inputs
{
    public class UserInput
    {
        public required string Username { get; set; }

        public required string EmailAddress { get; set; }

        public required string Password { get; set; }

        public required string Fullname { get; set; }

        public IEnumerable<UserCompanyInput> UserCompanies { get; set; } = [];
    }

    public class UserCompanyInput
    {
        public Guid? UserCompanyId { get; set; }

        public Guid CompanyId { get; set; }

        public IEnumerable<Guid> RoleIds { get; set; } = [];
    }
}
