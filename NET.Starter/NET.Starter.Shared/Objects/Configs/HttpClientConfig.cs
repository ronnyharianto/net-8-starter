namespace NET.Starter.Shared.Objects.Configs
{
    /// <summary>
    /// Represents configuration settings for HttpClient.
    /// Defines the request timeout and the lifetime of the HttpClient handler.
    /// </summary>
    public class HttpClientConfig
    {
        /// <summary>
        /// The maximum duration (in second(s)) an HTTP request can take before timing out.
        /// Default value: 60 seconds.
        /// </summary>
        public int Timeout { get; set; } = 60;

        /// <summary>
        /// The duration (in minute(s)) the HttpClient handler is retained before being disposed.
        /// Helps control connection pool reuse and lifecycle management.
        /// Default value: 5 minutes.
        /// </summary>
        public int HandlerLifetime { get; set; } = 5;
    }
}