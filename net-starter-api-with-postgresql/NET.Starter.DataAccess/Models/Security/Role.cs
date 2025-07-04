using NET.Starter.DataAccess.Bases;
using NET.Starter.Shared.Attributes;

namespace NET.Starter.DataAccess.Models.Security
{
    /// <summary>
    /// Represents a user role that defines permission access for a group of users.
    /// <para><b>Important constraints:</b></para>
    /// <list type="bullet">
    ///   <item><description><see cref="Code"/> is unique and required.</description></item>
    ///   <item><description>Supports soft deletion via <see cref="EntityBase.RowStatus"/>.</description></item>
    /// </list>
    /// </summary>
    [DatabaseSchema("security")]
    internal class Role : EntityBase
    {
        /// <summary>
        /// Maximum length: 50 characters.
        /// </summary>
        public required string Code { get; set; }

        public virtual ICollection<User> Users { get; set; } = [];

        public virtual ICollection<RolePermission> RolePermissions { get; set; } = [];

        public virtual ICollection<UserCompanyRole> UserCompanyRoles { get; set; } = [];
    }
}