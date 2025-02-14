namespace NET.Starter.Shared.Objects
{
    /// <summary>
    /// Represents the currently authenticated user.
    /// This class stores identity-related information extracted from the authentication token.
    /// </summary>
    public class CurrentUserAccessor
    {
        /// <summary>
        /// Gets or sets the unique identifier of the user.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Gets or sets the full name of the authenticated user.
        /// Example: "John Doe".
        /// </summary>
        public string FullName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the email address of the user.
        /// Example: "john.doe@example.com".
        /// </summary>
        public string EmailAddress { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the identifier for the user's FCM (Firebase Cloud Messaging) token.
        /// This is used for push notifications.
        /// </summary>
        //public Guid? UserFcmTokenId { get; set; }

        /// <summary>
        /// Gets or sets the list of permissions granted to the user.
        /// Example for general purpose: ["RefreshToken", "MyPermission", "IamAdministrator", etc].
        /// Example for specific menu: ["MasterData.Company.Menu", "MasterData.Company.View", "MasterData.Company.Create", etc].
        /// </summary>
        public IEnumerable<string>? Permissions { get; set; }
    }
}
