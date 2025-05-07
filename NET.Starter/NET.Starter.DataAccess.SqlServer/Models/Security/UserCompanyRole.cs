using NET.Starter.DataAccess.SqlServer.Bases;
using NET.Starter.Shared.Attributes;

namespace NET.Starter.DataAccess.SqlServer.Models.Security
{
    /// <summary>
    /// Represents the UserCompanyRole entity, which maps to the "UserCompanyRoles" table in the "Security" schema.
    /// This entity contains details about specific user & company with roles within the system.
    /// </summary>
    [DatabaseSchema("Security")]
    public class UserCompanyRole : EntityBase
    {
        /// <summary>
        /// Gets or sets the unique identifier of the user & company associated with this role.
        /// This property is required.
        /// </summary>
        public Guid UserCompanyId { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the role associated with the user.
        /// This property is required.
        /// </summary>
        public Guid RoleId { get; set; }

        /// <summary>
        /// Gets or sets the user & company associated with this role.
        /// This is a navigation property.
        /// </summary>
        public virtual UserCompany UserCompany { get; set; } = null!;

        /// <summary>
        /// Gets or sets the role associated with the user.
        /// This is a navigation property.
        /// </summary>
        public virtual Role Role { get; set; } = null!;
    }
}
