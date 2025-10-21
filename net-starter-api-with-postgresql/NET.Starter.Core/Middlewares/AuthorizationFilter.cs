using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using NET.Starter.Shared.Attributes;
using NET.Starter.Shared.Constants;
using NET.Starter.Shared.Objects;
using NET.Starter.Shared.Objects.Dtos;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;

namespace NET.Starter.Core.Middlewares
{
    /// <summary>
    /// Authorization middleware that enforces permission checks based on bearer token claims.
    /// Reads required permissions from <see cref="AppAuthorizeAttribute"/> and compares them to the user's token claims.
    /// </summary>
    internal class AuthorizationFilter(ILogger<AuthorizationFilter> _logger, CurrentUserAccessor _currentUserAccessor) : IAuthorizationFilter
    {
        /// <summary>
        /// Called by the framework to authorize an HTTP request.
        /// </summary>
        /// <param name="context">Authorization filter context containing HTTP context and endpoint metadata.</param>
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            try
            {
                var response = new BaseDto("Authorization has been denied for this request.", HttpStatusCode.Unauthorized)
                {
                    Id = context.HttpContext.TraceIdentifier
                };

                // Skip if endpoint allows anonymous access
                if (context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any())
                {
                    _logger.LogDebug("Skipping authorization (AllowAnonymousAttribute found).");

                    return;
                }

                var headerToken = context.HttpContext.Request.Headers.Authorization;
                _logger.LogDebug("Authorization header received: {Token}", string.IsNullOrWhiteSpace(headerToken) ? "[empty]" : "[token provided]");

                var identity = context.HttpContext.User.Identity as ClaimsIdentity;

                if (!(identity?.IsAuthenticated ?? false))
                {
                    _logger.LogWarning("User is not authenticated.");
                }
                else if (identity.Claims is { } claims && claims.Any())
                {
                    var permissions = identity.Claims.Where(i => i.Type == CustomJwtRegisteredClaimName.TypeCode);

                    if (IsAuthorize(context, permissions, _logger))
                    {
                        // Extract identity info and populate CurrentUserAccessor
                        _currentUserAccessor.UserId = new Guid(claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sid)?.Value ?? Guid.Empty.ToString());
                        _currentUserAccessor.FullName = claims.FirstOrDefault(c => c.Type == ClaimTypes.GivenName)?.Value ?? string.Empty;
                        _currentUserAccessor.EmailAddress = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value ?? string.Empty;
                        _currentUserAccessor.CompanyId = new Guid(claims.FirstOrDefault(c => c.Type == CustomJwtRegisteredClaimName.CurrentCompany)?.Value ?? Guid.Empty.ToString());
                        _currentUserAccessor.Permissions = permissions.Select(p => p.Value);

                        _logger.LogDebug("Authorization succeeded. UserId={UserId}, CompanyId={CompanyId}", _currentUserAccessor.UserId, _currentUserAccessor.CompanyId);

                        return;
                    }

                    _logger.LogWarning("User does not have required permissions.");
                }

                // If not authorized, return error response
                context.HttpContext.Response.StatusCode = response.Code;
                context.Result = new JsonResult(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during authorization.");

                var errorResponse = new BaseDto("An error occurred while authorizing the request.", HttpStatusCode.InternalServerError)
                {
                    Id = context.HttpContext.TraceIdentifier
                };

                context.HttpContext.Response.StatusCode = errorResponse.Code;
                context.Result = new JsonResult(errorResponse);
            }
        }

        /// <summary>
        /// Checks if the user has the required permission(s) to access the endpoint.
        /// </summary>
        /// <param name="context">The current authorization context.</param>
        /// <param name="claims">The user's permission claims.</param>
        /// <param name="logger">Logger instance.</param>
        /// <returns><c>true</c> if authorized; otherwise, <c>false</c>.</returns>
        private static bool IsAuthorize(AuthorizationFilterContext context, IEnumerable<Claim> claims, ILogger<AuthorizationFilter> logger)
        {
            var appAuthorizeAttribute = context.ActionDescriptor.EndpointMetadata.OfType<AppAuthorizeAttribute>().FirstOrDefault();
            if (appAuthorizeAttribute is null)
            {
                logger.LogWarning("Authorization failed: No AppAuthorizeAttribute found.");
                return false;
            }
            else if (appAuthorizeAttribute.Permissions.Length == 0)
            {
                logger.LogInformation("Authorization successful: Allow to authenticated users.");

                return true;
            }

            var matched = claims.Where(c => appAuthorizeAttribute.Permissions.Contains(c.Value, StringComparer.Ordinal)).Select(c => c.Value);
            if (matched.Any())
            {
                logger.LogInformation("Authorization successful: Matches permission(s): {Permissions}", string.Join(", ", matched));
                return true;
            }

            logger.LogWarning("Authorization failed: No matching permissions found.");
            return false;
        }
    }
}
