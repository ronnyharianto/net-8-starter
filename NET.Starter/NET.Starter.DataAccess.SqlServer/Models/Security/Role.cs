using NET.Starter.DataAccess.SqlServer.Bases;
using NET.Starter.Shared.Attributes;

namespace NET.Starter.DataAccess.SqlServer.Models.Security
{
    /// <summary>
    /// Represents the Role entity, which maps to the "Roles" table in the "Security" schema.
    /// This entity contains details about specific roles within the system.
    /// </summary>
    [DatabaseSchema("Security")]
    public class Role : EntityBase
    {
        /// <summary>
        /// Gets or sets the unique role code that identifies a specific role.
        /// This property is required.
        /// </summary>
        public required string RoleCode { get; set; }

        /// <summary>
        /// Gets or sets the user roles associated with a specific role.
        /// This is a navigation property.
        /// </summary>
        public virtual ICollection<UserRole> UserRoles { get; set; } = [];

        /// <summary>
        /// Gets or sets the role permissions associated with a specific role.
        /// This is a navigation property.
        /// </summary>
        public virtual ICollection<RolePermission> RolePermissions { get; set; } = [];
    }
}