using NET.Starter.DataAccess.SqlServer.Bases;
using NET.Starter.Shared.Attributes;

namespace NET.Starter.DataAccess.SqlServer.Models.Security
{
    /// <summary>
    /// Represents the Permission entity, which maps to the "Permissions" table in the "Security" schema.
    /// This entity contains details about specific permissions within the system.
    /// </summary>
    [DatabaseSchema("Security")]
    public class Permission : EntityBase
    {
        /// <summary>
        /// Gets or sets the unique code that identifies a specific permission.
        /// This property is required.
        /// </summary>
        public required string PermissionCode { get; set; }

        /// <summary>
        /// Gets or sets the role permissions associated with a specific permission.
        /// This is a navigation property.
        /// </summary>
        public virtual ICollection<RolePermission> RolePermissions { get; set; } = [];
    }
}
