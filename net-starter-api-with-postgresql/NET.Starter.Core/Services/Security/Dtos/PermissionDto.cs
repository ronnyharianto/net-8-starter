namespace NET.Starter.Core.Services.Security.Dtos
{
    public class PermissionDto
    {
        public Guid PermissionId { get; set; }

        public required string PermissionCode { get; set; }
    }
}
