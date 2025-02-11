namespace NET.Starter.Shared.Objects
{
    /// <summary>
    /// Represents security-related configuration settings for authentication and session management.
    /// </summary>
    public class SecurityConfig
    {
        /// <summary>
        /// Gets or sets the maximum number of failed login attempts before an account is locked or restricted.
        /// </summary>
        public int MaximumLoginRetry { get; set; }

        /// <summary>
        /// Gets or sets the token issuer, typically the domain or name of the authentication provider.
        /// Example: "https://yourapp.com"
        /// </summary>
        public string Issuer { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the audience that the token is intended for.
        /// This should match the client application consuming the authentication.
        /// Example: "your-client-id"
        /// </summary>
        public string Audience { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the secret key used for signing JWT tokens.
        /// Ensure this key is strong and securely stored.
        /// </summary>
        public string SecretKey { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the token expiration time in minutes.
        /// Defines how long an authentication token remains valid before it expires.
        /// Example: 15 (for 15 minutes)
        /// </summary>
        public int TokenExpired { get; set; }

        /// <summary>
        /// Gets or sets the session expiration time in days.
        /// Determines how long a user session remains active before requiring re-authentication.
        /// Example: 7 (for 7 days)
        /// </summary>
        public int SessionExpired { get; set; }
    }
}
