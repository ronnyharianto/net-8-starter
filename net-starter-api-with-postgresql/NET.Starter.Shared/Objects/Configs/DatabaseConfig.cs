namespace NET.Starter.Shared.Objects.Configs
{
    /// <summary>
    /// Configuration settings for connecting to a server database.
    /// </summary>
    public class DatabaseConfig
    {
        /// <summary>
        /// The connection string for the server database.
        /// </summary>
        /// <remarks>
        /// Defaults to an empty string. Should be set to a valid connection string
        /// from application configuration.
        /// </remarks>
        public string ConnectionString { get; set; } = string.Empty;

        /// <summary>
        /// The timeout duration in seconds for database commands.
        /// </summary>
        /// <remarks>
        /// Defaults to 60 seconds. Adjust this value based on expected query execution time.
        /// </remarks>
        public int CommandTimeout { get; set; } = 60;
    }
}
