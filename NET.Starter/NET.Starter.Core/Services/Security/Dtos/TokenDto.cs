namespace NET.Starter.Core.Services.Security.Dtos
{
    /// <summary>
    /// Represents a Data Transfer Object (DTO) for token information.
    /// </summary>
    public class TokenDto
    {
        /// <summary>
        /// The JWT access token used for authenticating API requests.
        /// </summary>
        public required string AccessToken { get; set; }

        /// <summary>
        /// The expiration date and time of the access token.
        /// </summary>
        public required DateTime ExpiredAt { get; set; }

        /// <summary>
        /// The refresh token used to obtain a new access token when the current one expires.
        /// </summary>
        public required string RefreshToken { get; set; }

        /// <summary>
        /// The expiration date and time of the session associated with the refresh token.
        /// </summary>
        public required DateTime SessionExpiredAt { get; set; }
    }
}
