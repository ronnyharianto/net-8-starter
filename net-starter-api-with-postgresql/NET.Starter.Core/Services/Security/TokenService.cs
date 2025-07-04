using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NET.Starter.Core.Services.Security.CustomModels;
using NET.Starter.DataAccess.Models.Security;
using NET.Starter.Shared.Constants;
using NET.Starter.Shared.Objects.Configs;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace NET.Starter.Core.Services.Security
{
    /// <summary>
    /// Provides methods for generating JWT access and refresh tokens for authenticated users.
    /// </summary>
    internal class TokenService(ILogger<TokenService> _logger, IOptions<AuthenticationConfig> _securityOption)
    {
        /// <summary>
        /// Generates an access token and a refresh token for the given user with specified permissions.
        /// </summary>
        /// <param name="dataUser">The user for whom the tokens are generated.</param>
        /// <param name="companyId">The company identifier associated with the token.</param>
        /// <param name="permissions">A list of permissions assigned to the user, used to build claims for the access token.</param>
        /// <returns>
        /// A <see cref="TokenResult"/> containing the generated access token and refresh token along with their expiration times.
        /// </returns>
        internal TokenResult GenerateToken(User dataUser, Guid companyId, IEnumerable<string> permissions)
        {
            _logger.LogInformation("Starting token generation for user Id: {UserId} to access company Id: {CompanyId}, with permissions: {Permissions}",
                dataUser.Id, companyId, permissions);

            var accessTokenExpireAt = DateTime.UtcNow.AddMinutes(_securityOption.Value.JwtOption.TokenExpired);
            var refreshTokenExpireAt = DateTime.UtcNow.AddDays(_securityOption.Value.JwtOption.SessionExpired);

            var accessToken = CreateSecurity(dataUser, accessTokenExpireAt, companyId, permissions);
            _logger.LogInformation("Successfully generated Access Token for user Id: {UserId} to access company Id: {CompanyId}", dataUser.Id, companyId);

            var refreshToken = CreateSecurity(dataUser, refreshTokenExpireAt, companyId, [PermissionConstants.RefreshToken]);
            _logger.LogInformation("Successfully generated Refresh Token for user Id: {UserId} to access company Id: {CompanyId}", dataUser.Id, companyId);

            return new()
            {
                Token = accessToken,
                TokenExpiresAt = accessTokenExpireAt,
                Refresh = refreshToken,
                RefreshExpiresAt = refreshTokenExpireAt
            };
        }

        /// <summary>
        /// Creates a JWT for the user with the given expiration time and permissions.
        /// </summary>
        /// <param name="dataUser">The user for whom the token is created.</param>
        /// <param name="expireAt">The expiration time of the token.</param>
        /// <param name="companyId">The company ID associated with the token.</param>
        /// <param name="permissions">A list of permissions assigned to the user, used to build claims for the token.</param>
        /// <returns>A JWT string representing the generated token.</returns>
        private string CreateSecurity(User dataUser, DateTime expireAt, Guid companyId, IEnumerable<string> permissions)
        {
            var secretKey = Encoding.ASCII.GetBytes(_securityOption.Value.JwtOption.SecretKey);

            var securityTokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new(
                [
                    new(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                    new(JwtRegisteredClaimNames.Email, dataUser.EmailAddress),
                    new(JwtRegisteredClaimNames.GivenName, dataUser.FullName),
                    new(JwtRegisteredClaimNames.Sid, dataUser.Id.ToString()),
                    new(CustomClaimTypeConstants.CurrentCompany, companyId.ToString()),
                ]),
                Expires = expireAt,
                Issuer = _securityOption.Value.JwtOption.Issuer,
                Audience = _securityOption.Value.JwtOption.Audience,
                SigningCredentials = new(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha512Signature),
                Claims = new Dictionary<string, object>
                {
                    { CustomClaimTypeConstants.TypeCode, permissions.ToList() }
                }
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(securityTokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
