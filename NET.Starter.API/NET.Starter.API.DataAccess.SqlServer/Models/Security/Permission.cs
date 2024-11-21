using NET.Starter.API.DataAccess.SqlServer.Bases;

namespace NET.Starter.API.DataAccess.SqlServer.Models.Security
{
    public partial class Permission : EntityBase
    {
        public required string PermissionCode { get; set; }
        public required string PermissionDescription { get; set; }
    }
}
