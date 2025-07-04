using NET.Starter.DataAccess.Bases;
using NET.Starter.Shared.Attributes;

namespace NET.Starter.DataAccess.Models.Security
{
    /// <summary>
    /// Represents an individual permission with a unique code.
    /// <para><b>Important constraints:</b></para>
    /// <list type="bullet">
    ///   <item><description><see cref="Code"/> is unique and required.</description></item>
    ///   <item><description>Supports soft deletion via <see cref="EntityBase.RowStatus"/>.</description></item>
    /// </list>
    /// </summary>
    [DatabaseSchema("security")]
    internal class Permission : EntityBase
    {
        /// <summary>
        /// Maximum length: 100 characters.
        /// </summary>
        public required string Code { get; set; }

        public virtual ICollection<RolePermission> RolePermissions { get; set; } = [];
    }
}
