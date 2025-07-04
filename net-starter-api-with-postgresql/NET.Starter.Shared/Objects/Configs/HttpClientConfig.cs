namespace NET.Starter.Shared.Objects.Configs
{
    /// <summary>
    /// Represents configuration settings for <see cref="HttpClient"/>, 
    /// including request timeout and handler lifetime.
    /// </summary>
    public class HttpClientConfig
    {
        /// <summary>
        /// The request timeout duration, in seconds.
        /// Determines how long to wait before aborting an HTTP request.
        /// Default: 60 seconds.
        /// </summary>
        public int Timeout { get; set; } = 60;

        /// <summary>
        /// The lifetime of the underlying <see cref="HttpMessageHandler"/>, in minutes.
        /// Controls how long handler instances are cached and reused.
        /// Default: 5 minutes.
        /// </summary>
        public int HandlerLifetime { get; set; } = 5;
    }
}