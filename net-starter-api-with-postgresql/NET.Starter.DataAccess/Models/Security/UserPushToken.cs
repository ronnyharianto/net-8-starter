using NET.Starter.DataAccess.Bases;
using NET.Starter.Shared.Attributes;
using NET.Starter.Shared.Enums;

namespace NET.Starter.DataAccess.Models.Security
{
    /// <summary>
    /// Represents a Firebase Cloud Messaging (FCM) token associated with a user for push notifications.
    /// <para><b>Important constraints:</b></para>
    /// <list type="bullet">
    ///   <item><description>Combination of <see cref="Provider"/> and <see cref="PushToken"/> must be unique.</description></item>
    ///   <item><description>Each token is linked to one <see cref="User"/>.</description></item>
    ///   <item><description>Supports soft deletion via <see cref="EntityBase.RowStatus"/>.</description></item>
    /// </list>
    /// </summary>
    [DatabaseSchema("security")]
    internal class UserPushToken : EntityBase
    {
        /// <summary>
        /// The Id of the user who owns the FCM token.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Provider that issued this push token (e.g., Firebase, OneSignal).
        /// Stored as a string in the database.
        /// Maximum length: 20 characters.
        /// </summary>
        public PushProvider Provider { get; set; }

        /// <summary>
        /// Push token string used to send notifications to the user's device.
        /// </summary>
        public required string PushToken { get; set; }

        public virtual User User { get; set; } = null!;
    }
}
