using NET.Starter.DataAccess.SqlServer.Bases;
using NET.Starter.DataAccess.SqlServer.Models.Organization;
using NET.Starter.Shared.Attributes;

namespace NET.Starter.DataAccess.SqlServer.Models.Security
{
    /// <summary>
    /// Represents the UserCompany entity, which maps to the "UserCompanies" table in the "Security" schema.
    /// This entity contains details about specific mapping user to company within the system.
    /// </summary>
    [DatabaseSchema("Security")]
    public class UserCompany : EntityBase
    {
        /// <summary>
        /// Gets or sets the unique identifier for a specific user.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for a specific company.
        /// </summary>
        public Guid CompanyId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user company mapping is the default.
        /// </summary>
        public bool IsDefault { get; set; }

        /// <summary>
        /// Gets or sets the user associated with the user company mapping.
        /// </summary>
        public virtual User User { get; set; } = null!;

        /// <summary>
        /// Gets or sets the company associated with the user company mapping.
        /// </summary>
        public virtual Company Company { get; set; } = null!;

        /// <summary>
        /// Gets or sets the user company roles associated with the user company mapping.
        /// </summary>
        public virtual ICollection<UserCompanyRole> UserCompanyRoles { get; set; } = null!;
    }
}