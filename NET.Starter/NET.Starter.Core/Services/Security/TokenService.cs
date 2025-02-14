using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NET.Starter.Core.Bases;
using NET.Starter.Core.Services.Security.CustomModels;
using NET.Starter.Core.Services.Security.Dtos;
using NET.Starter.DataAccess.SqlServer;
using NET.Starter.DataAccess.SqlServer.Models.Security;
using NET.Starter.Shared.Constants;
using NET.Starter.Shared.Objects.Configs;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NET.Starter.Core.Services.Security
{
    /// <summary>
    /// Provides methods for generating JWT access and refresh tokens for authenticated users.
    /// </summary>
    /// <param name="dbContext">The database context used for database operations.</param>
    /// <param name="mapper">The mapper service for object mapping.</param>
    /// <param name="logger">The logger service for capturing logs specific to the derived service.</param>
    /// <param name="securityOption">Configuration options related to security settings, such as secret keys and token expiration times.</param>
    internal class TokenService(ApplicationDbContext dbContext, IMapper mapper, ILogger<TokenService> logger, IOptions<SecurityConfig> securityOption)
        : BaseService<TokenService>(dbContext, mapper, logger)
    {
        private readonly SecurityConfig _securityConfig = securityOption.Value;

        /// <summary>
        /// Generates an access token and a refresh token for the given user with specified permissions.
        /// </summary>
        /// <param name="dataUser">The user for whom the tokens are generated.</param>
        /// <param name="permissions">A list of permissions assigned to the user, used to build claims for the access token.</param>
        /// <returns>
        /// A <see cref="TokenResult"/> containing the generated access token, refresh token, and their expiration times.
        /// </returns>
        /// <remarks>
        /// - The access token is used for authentication and authorization within the application.
        /// - The refresh token is used to obtain a new access token when the current one expires.
        /// - Both tokens are generated securely and follow the defined expiration policies.
        /// </remarks>
        internal TokenResult GenerateToken(User dataUser, IEnumerable<string> permissions)
        {
            _logger.LogInformation("Starting Generate Token with email address => {EmailAddress}, list permission => {Permissions}", dataUser.EmailAddress, permissions);

            // Set expiration times for the access token and refresh token.
            var accessTokenExpireAt = DateTime.UtcNow.AddMinutes(_securityConfig.TokenExpired);
            var refreshTokenExpireAt = DateTime.UtcNow.AddDays(_securityConfig.SessionExpired);

            // Generate an access token with the specified permissions.
            var accessToken = CreateSecurity(dataUser, accessTokenExpireAt, permissions);
            _logger.LogInformation("Successfully generated {TokenType} for {EmailAddress}", "Access Token", dataUser.EmailAddress);

            // Generate a refresh token with a special permission for token refresh.
            var refreshToken = CreateSecurity(dataUser, refreshTokenExpireAt, [PermissionConstants.RefreshToken]);
            _logger.LogInformation("Successfully generated {TokenType} for {EmailAddress}", "Refresh Token", dataUser.EmailAddress);

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
        /// <param name="permissions">A list of permissions assigned to the user, used to build claims for the token.</param>
        /// <returns>A JWT string representing the generated token.</returns>
        private string CreateSecurity(User dataUser, DateTime expireAt, IEnumerable<string> permissions)
        {
            // Encode the secret key for signing the token.
            var secretKey = Encoding.ASCII.GetBytes(_securityConfig.SecretKey);

            // Define the token descriptor, including claims, expiration, and signing credentials.
            var securityTokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new(
                [
                    new(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                    new(JwtRegisteredClaimNames.Email, dataUser.EmailAddress),
                    new(JwtRegisteredClaimNames.GivenName, dataUser.Fullname),
                    new(JwtRegisteredClaimNames.Sid, dataUser.Id.ToString())
                ]),
                Expires = expireAt, // Set the token's expiration time.
                Issuer = _securityConfig.Issuer, // The token issuer.
                Audience = _securityConfig.Audience, // The token audience.
                SigningCredentials = new(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha512Signature), // Token signing algorithm.
                Claims = new Dictionary<string, object>
                {
                    { PermissionConstants.TypeCode, permissions.ToList() } // Add permissions as a custom claim.
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
