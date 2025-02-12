using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using NET.Starter.Core.Bases;
using NET.Starter.Core.Services.Security.Dtos;
using NET.Starter.DataAccess.SqlServer;
using NET.Starter.Shared.Constants;
using NET.Starter.Shared.Objects.Configs;
using NET.Starter.DataAccess.SqlServer.Models.Security;
using System.Security.Claims;

namespace NET.Starter.Core.Services.Security
{
    /// <summary>
    /// Provides methods for generating JWT access and refresh tokens for authenticated users.
    /// </summary>
    /// <param name="dbContext">The database context used for database operations.</param>
    /// <param name="mapper">The mapper service for object mapping.</param>
    /// <param name="logger">The logger service for capturing logs specific to the derived service.</param>
    /// <param name="securityOption">Configuration options related to security settings, such as secret keys and token expiration times.</param>
    public class TokenService(ApplicationDbContext dbContext, IMapper mapper, ILogger<TokenService> logger, IOptions<SecurityConfig> securityOption)
        : BaseService<TokenService>(dbContext, mapper, logger)
    {
        // Configuration settings related to security (e.g., secret keys, token expiration).
        private readonly SecurityConfig _securityConfig = securityOption.Value;

        /// <summary>
        /// Generates an access token and a refresh token for the given user with specified permissions.
        /// </summary>
        /// <param name="dataUser">The user for whom the tokens are generated.</param>
        /// <param name="permissions">List of permissions assigned to the user.</param>
        /// <returns>A TokenDto containing the access token, refresh token, and their expiration times.</returns>
        public TokenDto GenerateToken(User dataUser, List<string> permissions)
        {
            // Log the beginning of the token generation process.
            _logger.LogInformation("Start Generate Token with email address => {EmailAddress}, list permission => {Permissions}", dataUser.EmailAddress, permissions);

            // Set expiration times for the access token and refresh token.
            var accessTokenExpireAt = DateTime.UtcNow.AddMinutes(_securityConfig.TokenExpired);
            var refreshTokenExpireAt = DateTime.UtcNow.AddDays(_securityConfig.SessionExpired);

            // Generate an access token with the specified permissions.
            var accessToken = CreateSecurity(dataUser, accessTokenExpireAt, permissions);
            _logger.LogInformation("Successfully generated {TokenType} Token for {EmailAddress}", "Access", dataUser.EmailAddress);

            // Generate a refresh token with a special permission for token refresh.
            var refreshToken = CreateSecurity(dataUser, refreshTokenExpireAt, new List<string> { PermissionConstants.RefreshToken });
            _logger.LogInformation("Successfully generated {TokenType} Token for {EmailAddress}", "Refresh", dataUser.EmailAddress);

            // Return the generated tokens along with their expiration times.
            return new()
            {
                AccessToken = accessToken,
                ExpiredAt = accessTokenExpireAt,
                RefreshToken = refreshToken,
                SessionExpiredAt = refreshTokenExpireAt
            };
        }

        /// <summary>
        /// Creates a JWT for the user with the given expiration time and permissions.
        /// </summary>
        /// <param name="dataUser">The user for whom the token is created.</param>
        /// <param name="expireAt">The expiration time of the token.</param>
        /// <param name="permissions">List of permissions assigned to the token.</param>
        /// <returns>A JWT string representing the generated token.</returns>
        private string CreateSecurity(User dataUser, DateTime expireAt, List<string> permissions)
        {
            // Encode the secret key for signing the token.
            var secretKey = Encoding.ASCII.GetBytes(_securityConfig.SecretKey);

            // Define the token descriptor, including claims, expiration, and signing credentials.
            var securityTokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()), // Issued at timestamp.
                    new Claim(JwtRegisteredClaimNames.Email, dataUser.EmailAddress),    // User's email address.
                    new Claim(JwtRegisteredClaimNames.GivenName, dataUser.Fullname),    // User's full name.
                    new Claim(JwtRegisteredClaimNames.Sid, dataUser.Id.ToString())      // User's unique ID.
                }),
                Expires = expireAt, // Set the token's expiration time.
                Issuer = _securityConfig.Issuer, // The token issuer.
                Audience = _securityConfig.Audience, // The token audience.
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha512Signature), // Token signing algorithm.
                Claims = new Dictionary<string, object>
                {
                    { PermissionConstants.TypeCode, permissions } // Add permissions as a custom claim.
                }
            };

            // Create and write the JWT using a token handler.
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(securityTokenDescriptor);

            // Return the token as a string.
            return tokenHandler.WriteToken(token);
        }
    }
}
