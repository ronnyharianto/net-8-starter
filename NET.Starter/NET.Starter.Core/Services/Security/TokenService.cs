using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NET.Starter.Core.Bases;
using NET.Starter.Core.Services.Security.CustomModels;
using NET.Starter.DataAccess.SqlServer;
using NET.Starter.DataAccess.SqlServer.Models.Security;
using NET.Starter.Shared.Constants;
using NET.Starter.Shared.Objects.Configs;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace NET.Starter.Core.Services.Security
{
    /// <summary>
    /// Provides methods for generating JWT access and refresh tokens for authenticated users.
    /// </summary>
    internal class TokenService(
        ApplicationDbContext dbContext, 
        IMapper mapper, 
        ILogger<TokenService> logger, 
        IOptions<SecurityConfig> securityOption) : BaseService<TokenService>(dbContext, mapper, logger)
    {
        private readonly SecurityConfig _securityConfig = securityOption.Value;

        /// <summary>
        /// Generates an access token and a refresh token for the given user with specified permissions.
        /// </summary>
        /// <param name="dataUser">The user for whom the tokens are generated.</param>
        /// <param name="permissions">A list of permissions assigned to the user, used to build claims for the access token.</param>
        /// <returns>
        /// A <see cref="TokenResult"/> containing the generated access token and refresh token along with their expiration times.
        /// </returns>
        internal TokenResult GenerateToken(User dataUser, Guid companyId, IEnumerable<string> permissions)
        {
            _logger.LogInformation("Starting token generation for user Id: {UserId} to access company Id: {CompanyId}, with permission: {Permissions}", dataUser.Id, companyId, permissions);

            var accessTokenExpireAt = DateTime.UtcNow.AddMinutes(_securityConfig.TokenExpired);
            var refreshTokenExpireAt = DateTime.UtcNow.AddDays(_securityConfig.SessionExpired);

            var accessToken = CreateSecurity(dataUser, accessTokenExpireAt, companyId, permissions);
            _logger.LogInformation("Successfully generated {TokenType} for user Id: {UserId} to access company Id: {CompanyId}", "Access Token", dataUser.Id, companyId);

            var refreshToken = CreateSecurity(dataUser, refreshTokenExpireAt, companyId, [PermissionConstants.RefreshToken]);
            _logger.LogInformation("Successfully generated {TokenType} for user Id: {UserId} to access company Id: {CompanyId}", "Refresh Token", dataUser.Id, companyId);

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
        /// <param name="companyId">The company ID associated with the token.</param>
        /// <param name="permissions">A list of permissions assigned to the user, used to build claims for the token.</param>
        /// <returns>A JWT string representing the generated token.</returns>
        private string CreateSecurity(User dataUser, DateTime expireAt, Guid companyId, IEnumerable<string> permissions)
        {
            var secretKey = Encoding.ASCII.GetBytes(_securityConfig.SecretKey);

            var securityTokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new(
                [
                    new(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                    new(JwtRegisteredClaimNames.Email, dataUser.EmailAddress),
                    new(JwtRegisteredClaimNames.GivenName, dataUser.Fullname),
                    new(JwtRegisteredClaimNames.Sid, dataUser.Id.ToString()),
                    new(CustomClaimTypeConstants.Company, companyId.ToString()),
                ]),
                Expires = expireAt,
                Issuer = _securityConfig.Issuer,
                Audience = _securityConfig.Audience,
                SigningCredentials = new(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha512Signature),
                Claims = new Dictionary<string, object>
                {
                    { PermissionConstants.TypeCode, permissions.ToList() }
                }
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(securityTokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
