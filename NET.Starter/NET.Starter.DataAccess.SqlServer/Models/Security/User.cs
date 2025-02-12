using NET.Starter.DataAccess.SqlServer.Bases;
using NET.Starter.Shared.Attributes;

namespace NET.Starter.DataAccess.SqlServer.Models.Security
{
    /// <summary>
    /// Represents the User entity, which maps to the "Users" table in the "Security" schema.
    /// This entity contains details about specific users within the system.
    /// </summary>
    [DatabaseSchema("Security")]
    public class User : EntityBase
    {
        /// <summary>
        /// Gets or sets the unique username that identifies a specific user.
        /// This property is required.
        /// </summary>
        public required string Username { get; set; }

        /// <summary>
        /// Gets or sets the email address associated with a specific user.
        /// This property is required.
        /// </summary>
        public required string EmailAddress { get; set; }

        /// <summary>
        /// Gets or sets the password associated with a specific user.
        /// This property is required.
        /// </summary>
        public required string Password { get; set; }

        /// <summary>
        /// Gets or sets the full name of the user.
        /// This property is required.
        /// </summary>
        public required string Fullname { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user is active or not.
        /// Default value is true.
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Gets or sets the bad password count for a specific user.
        /// Default value is 0.
        /// </summary>
        public int BadPasswordCount { get; set; } = 0;

        /// <summary>
        /// Gets or sets the user roles associated with a specific user.
        /// This is a navigation property.
        /// </summary>
        public virtual ICollection<UserRole> UserRoles { get; set; } = [];
    }
}