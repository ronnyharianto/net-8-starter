using NET.Starter.DataAccess.SqlServer.Bases;
using NET.Starter.Shared.Attributes;

namespace NET.Starter.DataAccess.SqlServer.Models.Security
{
    /// <summary>
    /// Represents the RolePermission entity, which maps to the "RolePermissions" table in the "Security" schema.
    /// This entity contains details about specific role permissions within the system.
    /// </summary>
    [DatabaseSchema("Security")]
    public class RolePermission : EntityBase
    {
        /// <summary>
        /// Gets or sets the unique identifier of the role associated with the permission.
        /// This property is required.
        /// </summary>
        public required Guid RoleId { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the permission associated with the role.
        /// This property is required.
        /// </summary>
        public required Guid PermissionId { get; set; }

        /// <summary>
        /// Gets or sets the role associated with the user.
        /// This is a navigation property.
        /// </summary>
        public virtual Role Role { get; set; } = null!;

        /// <summary>
        /// Gets or sets the permission associated with the role.
        /// This is a navigation property.
        /// </summary>
        public virtual Permission Permission { get; set; } = null!;
    }
}
