using Serilog.Sinks.Grafana.Loki;

namespace NET.Starter.Shared.Objects.Configs
{
    /// <summary>
    /// Represents the logging configuration settings for the application,
    /// including external log providers such as Grafana Loki and Google Cloud Logging.
    /// </summary>
    public class LoggingConfig
    {
        /// <summary>
        /// Configuration for logging to Grafana Loki.
        /// </summary>
        public GrafanaLoki? GrafanaLoki { get; set; }

        /// <summary>
        /// Configuration for logging to Google Cloud Logging.
        /// </summary>
        public GoogleMonitoring? GoogleMonitoring { get; set; }
    }

    /// <summary>
    /// Contains settings for sending logs to Grafana Loki.
    /// </summary>
    public class GrafanaLoki
    {
        /// <summary>
        /// The endpoint URL of the Grafana Loki server.
        /// <para>Example: <c>http://127.0.0.1:3100</c></para>
        /// </summary>
        public string EndpointUrl { get; set; } = string.Empty;

        /// <summary>
        /// A collection of labels used to categorize logs in Loki.
        /// Labels enable filtering and querying in Grafana dashboards.
        /// <para>
        /// <code>
        /// Example:
        /// [
        ///     { "Key": "app", "Value": "my-app" },
        ///     { "Key": "env", "Value": "development" }
        /// ]
        /// </code>
        /// </para>
        /// </summary>
        public IEnumerable<LokiLabel>? LokiLabels { get; set; }
    }

    /// <summary>
    /// Represents the configuration for Google Cloud Logging integration.
    /// </summary>
    public class GoogleMonitoring
    {
        /// <summary>
        /// The Google Cloud project ID where logs will be sent.
        /// </summary>
        public string ProjectId { get; set; } = string.Empty;

        /// <summary>
        /// The name of the log stream within Google Cloud Logging.
        /// </summary>
        public string LogName { get; set; } = string.Empty;

        /// <summary>
        /// The service account credentials used for authentication with Google Cloud Logging.
        /// </summary>
        public GoogleServiceAccount ServiceAccount { get; set; } = null!;
    }
}
