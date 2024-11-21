using NET.Starter.DataAccess.SqlServer.Bases;
using NET.Starter.Shared.Attributes;

namespace NET.Starter.DataAccess.SqlServer.Models.Security
{
    [DatabaseSchema("Master")]
    public class Permission : EntityBase
    {
        public required string PermissionCode { get; set; }
        public required string PermissionDescription { get; set; }
    }
}
