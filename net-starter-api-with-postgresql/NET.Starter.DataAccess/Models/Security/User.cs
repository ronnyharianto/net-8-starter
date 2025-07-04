using NET.Starter.DataAccess.Bases;
using NET.Starter.Shared.Attributes;

namespace NET.Starter.DataAccess.Models.Security
{
    /// <summary>
    /// Represents a system user and authentication-related information.
    /// <para><b>Important constraints:</b></para>
    /// <list type="bullet">
    ///   <item><description><see cref="Username"/> and <see cref="EmailAddress"/> must be unique and are required.</description></item>
    ///   <item><description><see cref="Password"/> is hashed before being stored.</description></item>
    ///   <item><description><see cref="RoleId"/> defines the user's assigned role.</description></item>
    ///   <item><description>Supports soft deletion via <see cref="EntityBase.RowStatus"/>.</description></item>
    /// </list>
    /// </summary>
    [DatabaseSchema("security")]
    internal class User : EntityBase
    {
        /// <summary>
        /// Maximum length: 50 characters.
        /// </summary>
        public required string Username { get; set; }

        /// <summary>
        /// Maximum length: 50 characters.
        /// </summary>
        public required string EmailAddress { get; set; }

        public required string Password { get; set; }

        /// <summary>
        /// Maximum length: 50 characters.
        /// </summary>
        public string FullName { get; set; } = string.Empty;

        public string PictureUrl { get; set; } = string.Empty;

        /// <summary>
        /// Default Value: true
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Number of consecutive failed login attempts.
        /// </summary>
        public int BadPasswordCount { get; set; } = 0;

        /// <summary>
        /// If set, indicates the account is locked until this date (UTC).
        /// </summary>
        public DateTime? LockedUntil { get; set; }

        public virtual ICollection<UserPushToken> UserPushTokens { get; set; } = [];

        public virtual ICollection<UserCompany> UserCompanies { get; set; } = [];
    }
}