using FirebaseAdmin.Messaging;
using Serilog;
using System.Collections.ObjectModel;

namespace NET.Starter.Shared.Helpers
{
    /// <summary>
    /// Helper methods for sending notifications via Firebase Cloud Messaging (FCM).
    /// </summary>
    public static class FirebaseMessagingHelper
    {
        private static readonly FirebaseMessaging _messaging = FirebaseMessaging.DefaultInstance;

        /// <summary>
        /// Sends a notification message to a single device using its FCM token.
        /// </summary>
        /// <param name="fcmToken">The target device's FCM token.</param>
        /// <param name="title">The notification's title.</param>
        /// <param name="body">The notification's body text.</param>
        /// <param name="data">Optional custom data payload.</param>
        /// <returns>The message Id string if the message was successfully sent.</returns>
        public static async Task<string> SendToTokenAsync(string fcmToken, string title, string body, IDictionary<string, string>? data = null)
        {
            Log.Logger.Information("Sending notification to token {FcmToken} with title '{Title}'.", fcmToken, title);

            var message = new Message
            {
                Token = fcmToken,
                Notification = new Notification
                {
                    Title = title,
                    Body = body
                },
                Data = new ReadOnlyDictionary<string, string>(data ?? new Dictionary<string, string>())
            };

            var response = await _messaging.SendAsync(message);

            Log.Logger.Information("Notification sent successfully to token {FcmToken}. Message ID: {MessageId}", fcmToken, response);
            return response;
        }

        /// <summary>
        /// Sends a multicast notification message to multiple devices using their FCM tokens.
        /// </summary>
        /// <param name="fcmTokens">Collection of target device FCM tokens.</param>
        /// <param name="title">The notification's title.</param>
        /// <param name="body">The notification's body text.</param>
        /// <param name="data">Optional custom data payload.</param>
        /// <returns>A <see cref="BatchResponse"/> with the send results for each token.</returns>
        public static async Task<BatchResponse> SendMulticastAsync(IEnumerable<string> fcmTokens, string title, string body, IDictionary<string, string>? data = null)
        {
            Log.Logger.Information("Sending multicast notification to {TokenCount} tokens with title '{Title}'.", fcmTokens.Count(), title);

            var multicastMessage = new MulticastMessage
            {
                Tokens = [.. fcmTokens],
                Notification = new Notification
                {
                    Title = title,
                    Body = body
                },
                Data = new ReadOnlyDictionary<string, string>(data ?? new Dictionary<string, string>())
            };

            var response = await _messaging.SendEachForMulticastAsync(multicastMessage);

            Log.Logger.Information("Multicast notification sent. Success count: {SuccessCount}, Failure count: {FailureCount}", response.SuccessCount, response.FailureCount);
            return response;
        }

        /// <summary>
        /// Sends a data-only message to a single device using its FCM token.
        /// </summary>
        /// <param name="fcmToken">The target device's FCM token.</param>
        /// <param name="data">Custom key-value pairs to send in the data payload.</param>
        /// <returns>The message Id string if the message was successfully sent.</returns>
        public static async Task<string> SendDataOnlyToTokenAsync(string fcmToken, IDictionary<string, string> data)
        {
            Log.Logger.Information("Sending data-only message to token {FcmToken}.", fcmToken);

            var message = new Message
            {
                Token = fcmToken,
                Data = new ReadOnlyDictionary<string, string>(data ?? new Dictionary<string, string>())
            };

            var response = await _messaging.SendAsync(message);

            Log.Logger.Information("Data-only message sent successfully to token {FcmToken}. Message ID: {MessageId}", fcmToken, response);
            return response;
        }
    }
}
