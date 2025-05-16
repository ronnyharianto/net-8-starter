using NET.Starter.DataAccess.SqlServer.Bases;
using NET.Starter.Shared.Attributes;
using NET.Starter.Shared.Helpers;
using System.ComponentModel.DataAnnotations.Schema;

namespace NET.Starter.DataAccess.SqlServer.Models.Organization
{
    /// <summary>
    /// Represents the Branch entity, which maps to the "Branches" table in the "Organization" schema.
    /// This entity contains details about specific branches within the system.
    /// </summary>
    [DatabaseSchema("Organization")]
    public class Branch : EntityBase
    {
        /// <summary>
        /// Gets or sets the unique identifier of the company that owns this branch.
        /// </summary>
        public Guid CompanyId { get; set; }

        /// <summary>
        /// Gets or sets the unique code that identifies a specific branch.
        /// This property is required.
        /// </summary>
        public required string BranchCode { get; set; }

        /// <summary>
        /// Gets or sets the name of a specific branch.
        /// </summary>
        public required string BranchName { get; set; }

        [NotMapped]
        private string _branchTimeZone = string.Empty;

        /// <summary>
        /// Gets or sets the time zone ID for the branch.
        /// Must be a valid time zone ID as recognized by TimeZoneInfo.
        /// </summary>
        public string BranchTimeZone
        {
            get => _branchTimeZone;
            set
            {
                if (TimeZoneHelper.ValidateTimeZoneId(value) == false)
                {
                    throw new InvalidTimeZoneException($"Invalid time zone ID: {value}");
                }

                _branchTimeZone = value;
            }
        }

        /// <summary>
        /// Gets or sets the Company that owns this branch.
        /// </summary>
        public virtual Company Company { get; set; } = null!;
    }
}
