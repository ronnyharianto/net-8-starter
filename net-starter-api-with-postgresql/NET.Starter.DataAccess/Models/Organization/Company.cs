using NET.Starter.DataAccess.Bases;
using NET.Starter.DataAccess.Models.Security;
using NET.Starter.Shared.Attributes;

namespace NET.Starter.DataAccess.Models.Organization
{
    /// <summary>
    /// Represents a company with basic contact information.
    /// <para><b>Important constraints:</b></para>
    /// <list type="bullet">
    ///   <item><description><c>Code</c> is unique and required.</description></item>
    ///   <item><description><c>Name</c> is required.</description></item>
    ///   <item><description>Supports soft deletion via <see cref="EntityBase.RowStatus"/>.</description></item>
    /// </list>
    /// </summary>
    [DatabaseSchema("organization")]
    internal class Company : EntityBase
    {
        /// <summary>
        /// Maximum length: 15 characters.
        /// </summary>
        public required string Code { get; set; }

        /// <summary>
        /// Maximum length: 50 characters.
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// Maximum length: 150 characters.
        /// </summary>
        public string Address { get; set; } = string.Empty;

        /// <summary>
        /// Maximum length: 15 characters.
        /// </summary>
        public string PhoneNumber { get; set; } = string.Empty;

        /// <summary>
        /// Maximum length: 50 characters.
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Maximum length: 50 characters.
        /// </summary>
        public string Website { get; set; } = string.Empty;

        public virtual ICollection<UserCompany> UserCompanies { get; set; } = [];
    }
}
