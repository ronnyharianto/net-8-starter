using NET.Starter.DataAccess.Bases;
using NET.Starter.Shared.Attributes;

namespace NET.Starter.DataAccess.Models.Security
{
    /// <summary>
    /// Represents the relationship between a user-company assignment and a role.
    /// This enables assigning specific roles to users within the context of a company.
    /// <para><b>Important constraints:</b></para>
    /// <list type="bullet">
    ///   <item><description>Combination of <see cref="UserCompanyId"/> and <see cref="RoleId"/> must be unique.</description></item>
    ///   <item><description>Supports soft deletion via <see cref="EntityBase.RowStatus"/>.</description></item>
    /// </list>
    /// </summary>
    [DatabaseSchema("security")]
    internal class UserCompanyRole : EntityBase
    {
        /// <summary>
        /// Identifier of the associated user-company assignment.
        /// </summary>
        public Guid UserCompanyId { get; set; }

        /// <summary>
        /// Identifier of the associated role.
        /// </summary>
        public Guid RoleId { get; set; }

        public virtual UserCompany UserCompany { get; set; } = null!;

        public virtual Role Role { get; set; } = null!;
    }
}