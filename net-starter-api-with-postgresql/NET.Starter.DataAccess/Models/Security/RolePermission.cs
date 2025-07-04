using NET.Starter.DataAccess.Bases;
using NET.Starter.Shared.Attributes;

namespace NET.Starter.DataAccess.Models.Security
{
    /// <summary>
    /// Represents the relationship between a role and a permission.
    /// <para><b>Important constraints:</b></para>
    /// <list type="bullet">
    ///   <item><description>Combination of <see cref="RoleId"/> and <see cref="PermissionId"/> must be unique.</description></item>
    ///   <item><description>Supports soft deletion via <see cref="EntityBase.RowStatus"/>.</description></item>
    /// </list>
    /// </summary>
    [DatabaseSchema("security")]
    internal class RolePermission : EntityBase
    {
        /// <summary>
        /// Identifier of the associated role.
        /// </summary>
        public Guid RoleId { get; set; }

        /// <summary>
        /// Identifier of the associated permission.
        /// </summary>
        public Guid PermissionId { get; set; }

        public virtual Role Role { get; set; } = null!;

        public virtual Permission Permission { get; set; } = null!;
    }
}
