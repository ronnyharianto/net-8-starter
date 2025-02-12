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
    public class AuthorizationFilter(ILogger<AuthorizationFilter> logger, CurrentUserAccessor currentUserAccessor) : IAuthorizationFilter
    {
        private readonly ILogger<AuthorizationFilter> _logger = logger;
        private readonly string logPrefix = nameof(AuthorizationFilter);
        private readonly CurrentUserAccessor _currentUserAccessor = currentUserAccessor;

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var response = new BaseDto("Authorization has been denied for this request.", ResponseCode.UnAuthorized);
            var authorizationTokens = context.ActionDescriptor.EndpointMetadata.OfType<AuthorizationTokenAttribute>();

            if (SkipAuthorization(context))
            {
                _logger.LogInformation("{Prefix}: Allow anonymous access", logPrefix);
                return;
            }

            var headerToken = context.HttpContext.Request.Headers.Authorization;
            _logger.LogInformation("{Prefix}: {Token}", logPrefix, headerToken);

            try
            {
                if (authorizationTokens != null && authorizationTokens.Any())
                {
                    foreach (var authorizationToken in authorizationTokens)
                    {
                        if (headerToken.Equals(authorizationToken.Token)) return;
                    }
                }
                else if (context.HttpContext.User.Identity is ClaimsIdentity identity && identity.Claims != null && identity.Claims.Any())
                {
                    var permissions = identity.Claims.Where(i => i.Type == PermissionConstants.TypeCode);

                    if (IsAuthorize(context, permissions))
                    {
                        var sub = identity.Claims.FirstOrDefault(c => c.Type.Equals("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"))?.Value;
                        var sid = identity.Claims.FirstOrDefault(c => c.Type.Equals("sid"))?.Value;
                        var fcmId = identity.Claims.FirstOrDefault(c => c.Type.Equals("FcmId"))?.Value;

                        _currentUserAccessor.UserId = new Guid(sub ?? "00000000-0000-0000-0000-000000000000");
                        _currentUserAccessor.UserName = sid ?? _currentUserAccessor.UserName;
                        _currentUserAccessor.UserFcmTokenId = !string.IsNullOrWhiteSpace(fcmId) ? new Guid(fcmId) : null;
                        _currentUserAccessor.Permissions = permissions.Select(p => p.Value);

                        return;
                    }
                }

                _logger.LogError("{Prefix}: User does not have required permissions.", logPrefix);
                context.HttpContext.Response.StatusCode = 401;
                context.Result = new JsonResult(response);
            }
            catch (Exception ex)
            {
                _logger.LogError("{Prefix}: Error occurred while authorizing the request: {Message}", logPrefix, ex.Message);

                //context.HttpContext.Response.StatusCode = 401;
                //response.UnAuthorized(ex.Message);
                context.Result = new JsonResult(response);

                return;
            }
        }

        private static bool SkipAuthorization(AuthorizationFilterContext context) => context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();

        private static bool IsAuthorize(AuthorizationFilterContext context, IEnumerable<Claim> claims)
        {
            // If user doesn't have any permission then the user is not authorize
            if (!claims.Any()) return false;

            var appAuthorizeAttributes = context.ActionDescriptor.EndpointMetadata.OfType<AppAuthorizeAttribute>();

            foreach (var appAuthorizeAttribute in appAuthorizeAttributes)
            {
                if (claims.Any(c => appAuthorizeAttribute.Permissions.Contains(c.Value, StringComparer.OrdinalIgnoreCase))) return true;
            }

            return false;
        }
    }
}
