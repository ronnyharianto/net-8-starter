namespace NET.Starter.Shared.Objects.Configs
{
    /// <summary>
    /// Configuration settings for cloud-based messaging services.
    /// </summary>
    public class MessagingConfig
    {
        /// <summary>
        /// Firebase Cloud Messaging settings.
        /// </summary>
        public FirebaseMessaging FirebaseMessaging { get; set; } = null!;
    }

    /// <summary>
    /// Configuration details required to use Firebase Cloud Messaging.
    /// </summary>
    public class FirebaseMessaging
    {
        /// <summary>
        /// The Firebase project ID associated with the messaging service.
        /// </summary>
        public string ProjectId { get; set; } = string.Empty;

        /// <summary>
        /// Service account credentials used to authenticate with Firebase.
        /// </summary>
        public GoogleServiceAccount ServiceAccount { get; set; } = null!;
    }
}