namespace NET.Starter.Shared.Objects.Configs
{
    /// <summary>
    /// Represents the configuration settings for a SQL Server database.
    /// </summary>
    public class SqlServerConfig
    {
        /// <summary>
        /// Gets or sets the connection string used to connect to the SQL Server database.
        /// </summary>
        /// <remarks>
        /// The default value is an empty string. This property should be populated with
        /// a valid connection string from the application's configuration settings.
        /// </remarks>
        public string ConnectionString { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the timeout duration (in seconds) for database commands.
        /// </summary>
        /// <remarks>
        /// The default value is 60 seconds. Adjust this value based on the expected duration
        /// of long-running database queries or operations.
        /// </remarks>
        public int CommandTimeout { get; set; } = 60;
    }
}
