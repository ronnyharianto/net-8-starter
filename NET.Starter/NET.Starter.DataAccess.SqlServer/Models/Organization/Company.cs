using NET.Starter.DataAccess.SqlServer.Bases;
using NET.Starter.DataAccess.SqlServer.Models.Security;
using NET.Starter.Shared.Attributes;

namespace NET.Starter.DataAccess.SqlServer.Models.Organization
{
    /// <summary>
    /// Represents the Company entity, which maps to the "Companies" table in the "Organization" schema.
    /// This entity contains details about specific companies within the system.
    /// </summary>
    [DatabaseSchema("Organization")]
    public class Company : EntityBase
    {
        /// <summary>
        /// Gets or sets the unique code that identifies a specific company.
        /// This property is required.
        /// </summary>
        public required string CompanyCode { get; set; }

        /// <summary>
        /// Gets or sets the name of a specific company 
        /// </summary>
        public required string CompanyName { get; set; }

        /// <summary>
        /// Gets or sets the collection of branches associated with a specific company.
        /// </summary>
        public virtual ICollection<Branch> Branches { get; set; } = [];

        /// <summary>
        /// Gets or sets the collection of users associated with a specific company.
        /// </summary>
        public virtual ICollection<UserCompany> UserCompanies { get; set; } = [];
    }
}
