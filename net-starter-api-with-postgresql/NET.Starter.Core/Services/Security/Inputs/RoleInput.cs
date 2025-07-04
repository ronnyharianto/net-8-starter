namespace NET.Starter.Core.Services.Security.Inputs
{
    public class RoleInput
    {
        public required string Code { get; set; }

        public IEnumerable<Guid> PermissionIds { get; set; } = [];
    }
}
