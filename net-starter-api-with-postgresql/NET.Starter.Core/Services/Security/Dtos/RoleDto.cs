namespace NET.Starter.Core.Services.Security.Dtos
{
    public class RoleDto
    {
        public Guid RoleId { get; set; }

        public required string RoleCode { get; set; }

        public IEnumerable<PermissionDto> Permissions { get; set; } = [];
    }
}
