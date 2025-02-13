using NET.Starter.DataAccess.SqlServer.Bases;

namespace NET.Starter.DataAccess.SqlServer.Models.Security
{
    /// <summary>
    /// Represents the UserFcmToken entity, which maps to the "UserFcmTokens" table in the "Security" schema.
    /// This entity contains details about specific user FCM tokens within the system.
    /// </summary>
    public class UserFcmToken : EntityBase
    {
        /// <summary>
        /// The unique identifier of the user associated with the FCM token.
        /// This property is required.
        /// </summary>
        public required Guid UserId { get; set; }

        /// <summary>
        /// The FCM token associated with the user.
        /// This property is required.
        /// </summary>
        public required string FcmToken { get; set; }

        /// <summary>
        /// The user associated with the FCM token.
        /// This is a navigation property.
        /// </summary>
        public virtual User User { get; set; } = null!;
    }
}
