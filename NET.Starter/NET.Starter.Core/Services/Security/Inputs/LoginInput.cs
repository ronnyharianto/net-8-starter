namespace NET.Starter.Core.Services.Security.Inputs
{
    /// <summary>
    /// Represents the input required for the login process.
    /// </summary>
    public class LoginInput
    {
        /// <summary>
        /// Gets or sets the identifier of the user (e.g., username or email).
        /// </summary>
        public required string UserIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the password of the user.
        /// </summary>
        public required string Password { get; set; }

        /// <summary>
        /// Gets or sets the optional Firebase Cloud Messaging (FCM) token for push notifications.
        /// </summary>
        public string? FcmToken { get; set; }
    }
}
