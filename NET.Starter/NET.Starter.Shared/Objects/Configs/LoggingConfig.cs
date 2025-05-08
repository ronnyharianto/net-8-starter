using Serilog.Sinks.Grafana.Loki;

namespace NET.Starter.Shared.Objects.Configs
{
    /// <summary>
    /// Represents the logging configuration settings for the application.
    /// </summary>
    public class LoggingConfig
    {
        /// <summary>
        /// Grafana Loki configuration.
        /// </summary>
        public GrafanaLoki? GrafanaLoki { get; set; }
    }

    /// <summary>
    /// Contains settings for sending logs to Grafana Loki.
    /// </summary>
    public class GrafanaLoki
    {
        /// <summary>
        /// Endpoint URL for Grafana Loki, which is used as the log aggregation system.
        /// <para>Example: "http://127.0.0.1:3100".</para>
        /// </summary>
        public string EndpointUrl { get; set; } = string.Empty;

        /// <summary>
        /// list of labels to categorize logs in Loki. Labels help in filtering and searching logs in Grafana.
        /// <para>
        /// Example:
        /// [
        ///     { "Key": "app", "Value": "my-app" },
        ///     { "Key": "env", "Value": "development" }
        /// ]
        /// </para>
        /// </summary>
        public IEnumerable<LokiLabel>? LokiLabels { get; set; }
    }
}