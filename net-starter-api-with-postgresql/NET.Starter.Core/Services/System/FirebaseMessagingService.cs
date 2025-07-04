using FirebaseAdmin.Messaging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NET.Starter.DataAccess;
using NET.Starter.Shared.Enums;
using NET.Starter.Shared.Helpers;

namespace NET.Starter.Core.Services.System
{
    /// <summary>
    /// Service for sending Firebase Cloud Messaging notifications.
    /// </summary>
    internal class FirebaseMessagingService(ApplicationDbContext _dbContext, ILogger<FirebaseMessagingService> _logger)
    {
        /// <summary>
        /// Sends a multicast Firebase notification to the specified user.
        /// </summary>
        /// <param name="userId">User ID to whom the notification will be sent.</param>
        /// <param name="title">Notification title.</param>
        /// <param name="body">Notification body.</param>
        /// <param name="data">Optional additional data payload.</param>
        internal async Task SendMulticastAsync(Guid userId, string title, string body, IDictionary<string, string>? data = null)
        {
            _logger.LogInformation("Sending multicast notification to user ID: {UserId}", userId);

            var user = await _dbContext.Users
                                       .Include(u => u.UserPushTokens.Where(upt => upt.Provider == PushProvider.Firebase))
                                       .AsNoTracking()
                                       .FirstOrDefaultAsync(u => u.Id == userId);

            if (user is null || user.UserPushTokens.Count == 0)
            {
                _logger.LogWarning("No push tokens found for user ID: {UserId}", userId);
                return;
            }

            var pushTokens = user.UserPushTokens.Select(d => d.PushToken).ToList();
            var result = await FirebaseMessagingHelper.SendMulticastAsync(pushTokens, title, body, data);

            var failedTokens = result.Responses
                                     .Select((response, index) => new { response, Token = pushTokens[index] })
                                     .Where(x => !x.response.IsSuccess
                                            && (x.response.Exception?.MessagingErrorCode == MessagingErrorCode.Unregistered
                                             || x.response.Exception?.MessagingErrorCode == MessagingErrorCode.InvalidArgument))
                                     .ToList();

            if (failedTokens.Count > 0)
            {
                var failedTokenValues = failedTokens.Select(x => x.Token).ToList();

                _logger.LogWarning("Found {Count} invalid tokens for user ID: {UserId}. Tokens: {Tokens}",
                    failedTokens.Count, userId, string.Join(", ", failedTokenValues));

                var userPushTokens = await _dbContext.UserPushTokens
                                                    .Where(t => failedTokenValues.Contains(t.PushToken))
                                                    .ToListAsync();

                foreach (var userPushToken in userPushTokens)
                {
                    userPushToken.RowStatus = 1;
                }

                await _dbContext.SaveChangesAsync();
            }

            _logger.LogInformation("Finished sending notification to user ID: {UserId}. Success: {SuccessCount}, Failed: {FailureCount}",
                userId, result.SuccessCount, result.FailureCount);
        }
    }
}
