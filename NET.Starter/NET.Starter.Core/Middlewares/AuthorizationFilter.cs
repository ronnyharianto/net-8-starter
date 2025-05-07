using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using NET.Starter.Shared.Attributes;
using NET.Starter.Shared.Constants;
using NET.Starter.Shared.Enums;
using NET.Starter.Shared.Objects;
using NET.Starter.Shared.Objects.Dtos;
using System.Security.Claims;

namespace NET.Starter.Core.Middlewares
{
    /// <summary>
    /// Middleware filter to handle authorization logic for incoming requests.
    /// Ensures that users are authorized to access the requested resource by validating
    /// permissions, claims, and optionally, specific tokens defined in the endpoint metadata.
    /// If authorization fails, the request is denied with a 401 Unauthorized response.
    /// </summary>
    public class AuthorizationFilter(ILogger<AuthorizationFilter> logger, CurrentUserAccessor currentUserAccessor) : IAuthorizationFilter
    {
        private readonly ILogger<AuthorizationFilter> _logger = logger;
        private readonly CurrentUserAccessor _currentUserAccessor = currentUserAccessor;

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            try
            {
                var response = new BaseDto("Authorization has been denied for this request.", ResponseCode.UnAuthorized)
                {
                    Id = context.HttpContext.TraceIdentifier
                };

                if (context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any()) // Skip authorization if the endpoint allows anonymous access.
                {
                    _logger.LogWarning("Authorization skipped. Ensure permissions are configured appropriately if required.");

                    return;
                }

                var headerToken = context.HttpContext.Request.Headers.Authorization;
                _logger.LogInformation("Authorization header received. Token: {Token}", string.IsNullOrWhiteSpace(headerToken) ? "No token provided" : headerToken);

                var authorizationToken = context.ActionDescriptor.EndpointMetadata.OfType<AuthorizationTokenAttribute>().FirstOrDefault();
                if (authorizationToken != null) // If not null, the endpoint requires specific authorization tokens.
                {
                    if (authorizationToken.Token.Any(d => d.Equals(headerToken, StringComparison.Ordinal)))
                    {
                        _logger.LogInformation("Authorization valid for the provided token.");

                        return;
                    }
                }
                else if (!context.HttpContext.User.Identity?.IsAuthenticated ?? true) // If not authenticated, the user is not authorized.
                {
                    _logger.LogError("User is not authenticated.");
                }
                else if (context.HttpContext.User.Identity is ClaimsIdentity identity && identity.Claims != null && identity.Claims.Any())
                {
                    var permissions = identity.Claims.Where(i => i.Type == PermissionConstants.TypeCode);

                    if (IsAuthorize(context, permissions, _logger))
                    {
                        var fullName = identity.Claims.FirstOrDefault(c => c.Type.Equals("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname"))?.Value;
                        var emailAddress = identity.Claims.FirstOrDefault(c => c.Type.Equals("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress"))?.Value;
                        var sid = identity.Claims.FirstOrDefault(c => c.Type.Equals("sid"))?.Value;
                        var userTimeZone = identity.Claims.FirstOrDefault(c => c.Type.Equals(CustomClaimTypeConstants.TimeZone))?.Value;
                        var companyId = identity.Claims.FirstOrDefault(c => c.Type.Equals(CustomClaimTypeConstants.Company))?.Value;

                        _currentUserAccessor.UserId = new Guid(sid ?? "00000000-0000-0000-0000-000000000000");
                        _currentUserAccessor.FullName = fullName ?? string.Empty;
                        _currentUserAccessor.EmailAddress = emailAddress ?? string.Empty;
                        _currentUserAccessor.Permissions = permissions.Select(p => p.Value);
                        _currentUserAccessor.UserTimeZone = userTimeZone ?? "UTC";
                        _currentUserAccessor.CompanyId = new Guid(companyId ?? "00000000-0000-0000-0000-000000000000");

                        return;
                    }

                    _logger.LogError("User does not have required permissions.");
                }

                context.HttpContext.Response.StatusCode = response.Code;
                context.Result = new JsonResult(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing authorization.");

                var errorResponse = new BaseDto("An error occurred while authorizing the request.", ResponseCode.Error)
                {
                    Id = context.HttpContext.TraceIdentifier
                };

                context.HttpContext.Response.StatusCode = errorResponse.Code;
                context.Result = new JsonResult(errorResponse);
            }
        }

        /// <summary>
        /// Determines whether the user is authorized based on their claims and required permissions.
        /// </summary>
        /// <returns>True if the user is authorized, otherwise false.</returns>
        private static bool IsAuthorize(AuthorizationFilterContext context, IEnumerable<Claim> claims, ILogger<AuthorizationFilter> logger)
        {
            var appAuthorizeAttribute = context.ActionDescriptor.EndpointMetadata.OfType<AppAuthorizeAttribute>().FirstOrDefault();
            if (appAuthorizeAttribute == null) // If the endpoint doesn't have the AppAuthorize attribute, user is not authorized.
            {
                logger.LogWarning("Authorization failed: No AppAuthorize attribute found on the endpoint.");

                return false;
            }
            else if (appAuthorizeAttribute.Permissions.Length == 0) // If the endpoint doesn't require any permissions, user is authorized.
            {
                logger.LogInformation("Authorization successful: No permissions required.");

                return true;
            }

            var matchPermissions = claims.Where(c => appAuthorizeAttribute.Permissions.Contains(c.Value, StringComparer.Ordinal)).Select(c => c.Value);
            if (matchPermissions.Any()) // If the user has at least one matching permission, user is authorized.
            {
                logger.LogInformation("Authorization successful: Matches permission(s): {Permissions}", string.Join(", ", matchPermissions));

                return true;
            }

            logger.LogWarning("Authorization failed: No matching permissions found.");

            return false;
        }

    }
}
