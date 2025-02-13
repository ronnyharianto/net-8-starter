namespace NET.Starter.Core.Services.Security.CustomModels
{
    /// <summary>
    /// Represents the result of a token generation operation, including access and refresh tokens along with their expiration details.
    /// </summary>
    public class TokenResult
    {
        /// <summary>
        /// Gets or sets the JWT access token used for authenticating API requests.
        /// </summary>
        public required string AccessToken { get; set; }

        /// <summary>
        /// Gets or sets the expiration date and time of the access token.
        /// This defines the moment when the access token becomes invalid.
        /// </summary>
        public required DateTime ExpiredAt { get; set; }

        /// <summary>
        /// Gets or sets the refresh token used to obtain a new access token when the current one expires.
        /// The refresh token is designed to provide continuous access without requiring reauthentication.
        /// </summary>
        public required string RefreshToken { get; set; }

        /// <summary>
        /// Gets or sets the expiration date and time of the session associated with the refresh token.
        /// When this time is reached, the user will need to reauthenticate to continue accessing the system.
        /// </summary>
        public required DateTime SessionExpiredAt { get; set; }
    }

}
