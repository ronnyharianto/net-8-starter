using NET.Starter.DataAccess.SqlServer.Bases;
using NET.Starter.Shared.Attributes;

namespace NET.Starter.DataAccess.SqlServer.Models.Security
{
    /// <summary>
    /// Represents the UserRole entity, which maps to the "UserRoles" table in the "Security" schema.
    /// This entity contains details about specific user roles within the system.
    /// </summary>
    [DatabaseSchema("Security")]
    public class UserRole : EntityBase
    {
        /// <summary>
        /// Gets or sets the unique identifier of the user associated with this role.
        /// This property is required.
        /// </summary>
        public required Guid UserId { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the role associated with the user.
        /// This property is required.
        /// </summary>
        public required Guid RoleId { get; set; }

        /// <summary>
        /// Gets or sets the user associated with this role.
        /// This is a navigation property.
        /// </summary>
        public virtual User User { get; set; } = null!;

        /// <summary>
        /// Gets or sets the role associated with the user.
        /// This is a navigation property.
        /// </summary>
        public virtual Role Role { get; set; } = null!;
    }
}
