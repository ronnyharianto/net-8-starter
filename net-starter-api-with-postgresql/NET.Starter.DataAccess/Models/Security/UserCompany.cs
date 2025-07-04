using NET.Starter.DataAccess.Bases;
using NET.Starter.DataAccess.Models.Organization;
using NET.Starter.Shared.Attributes;

namespace NET.Starter.DataAccess.Models.Security
{
    /// <summary>
    /// Represents the relationship between a user and a company.
    /// <para><b>Important constraints:</b></para>
    /// <list type="bullet">
    ///   <item><description>Combination of <see cref="UserId"/> and <see cref="CompanyId"/> must be unique.</description></item>
    ///   <item><description>Each user can only have one default company.</description></item>
    ///   <item><description>Supports soft deletion via <see cref="EntityBase.RowStatus"/>.</description></item>
    /// </list>
    /// </summary>
    [DatabaseSchema("security")]
    internal class UserCompany : EntityBase
    {
        /// <summary>
        /// Identifier of the associated user.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Identifier of the associated company.
        /// </summary>
        public Guid CompanyId { get; set; }

        /// <summary>
        /// Indicates default company for the user.
        /// When user login, the default company will be selected.
        /// </summary>
        public bool IsDefault { get; set; }

        public virtual User User { get; set; } = null!;

        public virtual Company Company { get; set; } = null!;

        public virtual ICollection<UserCompanyRole> UserCompanyRoles { get; set; } = [];
    }
}