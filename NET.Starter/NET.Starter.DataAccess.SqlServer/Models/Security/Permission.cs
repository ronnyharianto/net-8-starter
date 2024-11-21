using NET.Starter.DataAccess.SqlServer.Bases;

namespace NET.Starter.DataAccess.SqlServer.Models.Security
{
    public partial class Permission : EntityBase
    {
        public required string PermissionCode { get; set; }
        public required string PermissionDescription { get; set; }
    }
}
