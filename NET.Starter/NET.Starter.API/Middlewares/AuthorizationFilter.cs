using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NET.Starter.Shared.Attributes;
using NET.Starter.Shared.Constants;
using NET.Starter.Shared.Enums;
using NET.Starter.Shared.Objects;
using NET.Starter.Shared.Objects.Dtos;
using System.Security.Claims;

namespace NET.Starter.API.Middlewares
{
    /// <summary>
    /// Middleware filter to handle authorization logic for incoming requests.
    /// Ensures that users are authorized to access the requested resource by validating
    /// permissions, claims, and optionally, specific tokens defined in the endpoint metadata.
    /// If authorization fails, the request is denied with a 401 Unauthorized response.
    /// </summary>
    /// <param name="logger">
    /// The logger instance used to log authorization activities, such as token validation,
    /// missing permissions, and any errors encountered during the process.
    /// </param>
    /// <param name="currentUserAccessor">
    /// An instance of <see cref="CurrentUserAccessor"/> used to manage and store details
    /// about the currently authenticated user, such as their ID, username, and permissions.
    /// </param>
    public class AuthorizationFilter(ILogger<AuthorizationFilter> logger, CurrentUserAccessor currentUserAccessor) : IAuthorizationFilter
    {
        private readonly ILogger<AuthorizationFilter> _logger = logger;
        private readonly CurrentUserAccessor _currentUserAccessor = currentUserAccessor;

        /// <summary>
        /// Executes the authorization logic when a request is being processed.
        /// </summary>
        /// <param name="context">The current authorization filter context.</param>
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            try
            {
                // Create a default unauthorized response DTO.
                var response = new BaseDto("Authorization has been denied for this request.", ResponseCode.UnAuthorized)
                {
                    Id = context.HttpContext.TraceIdentifier
                };

                // Skip authorization if the endpoint allows anonymous access.
                if (SkipAuthorization(context))
                {
                    _logger.LogWarning("Authorization skipped. Ensure permissions are configured appropriately if required.");

                    return;
                }

                // Retrieve the Authorization header from the HTTP request.
                var headerToken = context.HttpContext.Request.Headers.Authorization;
                _logger.LogInformation("Authorization header received. Token: {Token}", string.IsNullOrWhiteSpace(headerToken) ? "No token provided" : headerToken);

                // Retrieve custom authorization token attributes applied to the endpoint.
                var authorizationToken = context.ActionDescriptor.EndpointMetadata.OfType<AuthorizationTokenAttribute>().FirstOrDefault();

                // Check if the endpoint requires specific authorization tokens.
                if (authorizationToken != null)
                {
                    // If the provided token matches any required token, allow the request.
                    if (authorizationToken.Token.Any(d => d.Equals(headerToken, StringComparison.Ordinal)))
                    {
                        _logger.LogInformation("Authorization valid for the provided token.");

                        return;
                    }
                }
                // Check if the user has valid claims from their identity.
                else if (context.HttpContext.User.Identity is ClaimsIdentity identity && identity.Claims != null && identity.Claims.Any())
                {
                    // Extract permission claims for validation.
                    var permissions = identity.Claims.Where(i => i.Type == PermissionConstants.TypeCode);

                    // Verify if the user is authorized based on their permissions.
                    if (IsAuthorize(context, permissions, _logger))
                    {
                        // Retrieve user-specific claims.
                        var sub = identity.Claims.FirstOrDefault(c => c.Type.Equals("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"))?.Value;
                        var sid = identity.Claims.FirstOrDefault(c => c.Type.Equals("sid"))?.Value;
                        var fcmId = identity.Claims.FirstOrDefault(c => c.Type.Equals("FcmId"))?.Value;

                        // Set the current user context using the retrieved claims.
                        _currentUserAccessor.UserId = new Guid(sub ?? "00000000-0000-0000-0000-000000000000");
                        _currentUserAccessor.UserName = sid ?? _currentUserAccessor.UserName;
                        _currentUserAccessor.UserFcmTokenId = !string.IsNullOrWhiteSpace(fcmId) ? new Guid(fcmId) : null;
                        _currentUserAccessor.Permissions = permissions.Select(p => p.Value);

                        return; // User is authorized, allow the request.
                    }
                }

                // Log an error if the user lacks the required permissions.
                _logger.LogError("User does not have required permissions.");

                // Respond with an unauthorized response.
                context.HttpContext.Response.StatusCode = response.Code;
                context.Result = new JsonResult(response);
            }
            catch (Exception ex)
            {
                // Log any exceptions that occur during authorization.
                _logger.LogError(ex, "An error occurred while processing authorization.");

                var errorResponse = new BaseDto("An error occurred while authorizing the request.", ResponseCode.Error)
                {
                    Id = context.HttpContext.TraceIdentifier
                };

                // Respond with an internal server error response.
                context.HttpContext.Response.StatusCode = errorResponse.Code;
                context.Result = new JsonResult(errorResponse);
            }
        }

        /// <summary>
        /// Checks if the request should skip authorization based on the presence of <see cref="AllowAnonymousAttribute"/>.
        /// </summary>
        /// <param name="context">The current authorization filter context.</param>
        /// <returns>True if authorization can be skipped, otherwise false.</returns>
        private static bool SkipAuthorization(AuthorizationFilterContext context)
        {
            // Check if the endpoint has the [AllowAnonymous] attribute, indicating no authorization is required.
            return context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
        }

        /// <summary>
        /// Determines whether the user is authorized based on their claims and required permissions.
        /// </summary>
        /// <param name="context">The current authorization filter context.</param>
        /// <param name="claims">The user's claims.</param>
        /// <returns>True if the user is authorized, otherwise false.</returns>
        private static bool IsAuthorize(AuthorizationFilterContext context, IEnumerable<Claim> claims, ILogger<AuthorizationFilter> logger)
        {
            // If user doesn't have any claims, they are not authorized.
            if (!claims.Any())
            {
                logger.LogWarning("Authorization failed: No claims found for the user.");

                return false;
            }

            // Retrieve custom AppAuthorize attributes applied to the endpoint.
            var appAuthorizeAttribute = context.ActionDescriptor.EndpointMetadata.OfType<AppAuthorizeAttribute>().FirstOrDefault();
            if (appAuthorizeAttribute == null) // If the endpoint doesn't have the AppAuthorize attribute, user is not authorized.
            {
                logger.LogWarning("Authorization failed: No AppAuthorize attribute found on the endpoint.");

                return false;
            }

            // Check if the user's claims match any required permissions in the AppAuthorize attribute.
            var matchPermissions = claims.Where(c => appAuthorizeAttribute.Permissions.Contains(c.Value, StringComparer.Ordinal)).Select(c => c.Value);
            if (matchPermissions.Any())
            {
                logger.LogInformation("Authorization successful: Matches permission(s): {Permissions}", string.Join(", ", matchPermissions));

                return true;
            }

            logger.LogWarning("Authorization failed: No matching permissions found.");

            return false; // User is not authorized.
        }

    }
}
