namespace NET.Starter.Shared.Objects.Configs
{
    /// <summary>
    /// Represents authentication-related configuration, including JWT, login policy, and OAuth providers.
    /// </summary>
    public class AuthenticationConfig
    {
        /// <summary>
        /// Login attempt rules and account lockout policy.
        /// </summary>
        public LoginPolicy LoginPolicy { get; set; } = null!;

        /// <summary>
        /// Configuration options for JWT token generation and validation.
        /// </summary>
        public JwtOption JwtOption { get; set; } = null!;

        /// <summary>
        /// OAuth providers configuration (e.g., Google).
        /// </summary>
        public OAuthProvider OAuthProvider { get; set; } = null!;
    }

    /// <summary>
    /// Represents login security policies such as retry limits and auto-unlock timing.
    /// </summary>
    public class LoginPolicy
    {
        /// <summary>
        /// Maximum number of failed login attempts before locking the account. Default is 5.
        /// </summary>
        public int MaxLoginRetry { get; set; } = 5;

        /// <summary>
        /// Number of minutes before a locked account is automatically unlocked. Default is 15.
        /// </summary>
        public int AutoUnlockAfter { get; set; } = 15;
    }

    /// <summary>
    /// Configuration for issuing and validating JWT tokens.
    /// </summary>
    public class JwtOption
    {
        /// <summary>
        /// The token issuer (e.g., your domain or service name).
        /// </summary>
        public string Issuer { get; set; } = string.Empty;

        /// <summary>
        /// The intended recipient (audience) of the token.
        /// </summary>
        public string Audience { get; set; } = string.Empty;

        /// <summary>
        /// Secret key used to sign the JWT token.
        /// </summary>
        public string SecretKey { get; set; } = string.Empty;

        /// <summary>
        /// Token expiration time in minutes.
        /// </summary>
        public int TokenExpired { get; set; }

        /// <summary>
        /// Session expiration time in days.
        /// </summary>
        public int SessionExpired { get; set; }
    }

    /// <summary>
    /// Contains configuration for external OAuth providers.
    /// </summary>
    public class OAuthProvider
    {
        /// <summary>
        /// Configuration for Google OAuth provider.
        /// </summary>
        public GoogleOAuthOption Google { get; set; } = new();
    }

    /// <summary>
    /// Represents Google OAuth client settings.
    /// </summary>
    public class GoogleOAuthOption
    {
        /// <summary>
        /// Google Client ID for OAuth authentication.
        /// </summary>
        public string ClientId { get; set; } = string.Empty;

        /// <summary>
        /// Google Client Secret for OAuth authentication.
        /// </summary>
        public string ClientSecret { get; set; } = string.Empty;

        /// <summary>
        /// The callback path that handles Google's OAuth redirect.
        /// </summary>
        public string CallbackPath { get; set; } = string.Empty;
    }
}
