using Serilog.Sinks.Grafana.Loki;

namespace NET.Starter.Shared.Objects.Configs
{
    /// <summary>
    /// Represents the logging configuration settings for the application.
    /// </summary>
    public class LoggingConfig
    {
        /// <summary>
        /// Gets or sets the Grafana Loki configuration.
        /// </summary>
        public GrafanaLoki? GrafanaLoki { get; set; }
    }

    /// <summary>
    /// Contains settings for sending logs to Grafana Loki.
    /// </summary>
    public class GrafanaLoki
    {
        /// <summary>
        /// Gets or sets the endpoint URL for Grafana Loki, 
        /// which is used as the log aggregation system.
        /// Example: "http://127.0.0.1:3100".
        /// </summary>
        public string EndpointUrl { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the list of labels to categorize logs in Loki.
        /// Labels help in filtering and searching logs in Grafana.
        /// Example:
        /// [
        ///     { "Key": "app", "Value": "my-app" },
        ///     { "Key": "env", "Value": "development" }
        /// ]
        /// </summary>
        public IEnumerable<LokiLabel>? LokiLabels { get; set; }
    }
}