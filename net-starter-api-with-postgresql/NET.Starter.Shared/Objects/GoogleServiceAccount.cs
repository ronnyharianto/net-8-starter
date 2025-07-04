using Newtonsoft.Json;

namespace NET.Starter.Shared.Objects
{
    /// <summary>
    /// Represents the service account credentials used for authenticating Google Cloud Logging.
    /// </summary>
    public class GoogleServiceAccount
    {
        /// <summary>
        /// The type of service account (typically <c>service_account</c>).
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// The private key associated with the service account.
        /// </summary>
        [JsonProperty("private_key")]
        public string PrivateKey { get; set; } = string.Empty;

        /// <summary>
        /// The client email associated with the service account.
        /// </summary>
        [JsonProperty("client_email")]
        public string ClientEmail { get; set; } = string.Empty;
    }
}
