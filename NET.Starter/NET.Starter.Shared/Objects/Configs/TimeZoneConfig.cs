namespace NET.Starter.Shared.Objects.Configs
{
    /// <summary>
    /// Represents application-level configuration settings related to timezone handling.
    /// </summary>
    public class TimeZoneConfig
    {
        /// <summary>
        /// Gets or sets the timezone used by the backend system for datetime conversions.
        /// This is typically used to convert UTC timestamps to a local timezone for display purposes.
        /// Default value: "UTC"
        /// </summary>
        public string SystemTimeZone { get; set; } = "UTC";

        /// <summary>
        /// Gets or sets a value indicating whether timezone conversion is enabled.
        /// When disabled, datetime values are returned as UTC without conversion.
        /// Default value: false
        /// <para>If the frontend system handles timezone conversion, set this to <c>false</c></para>
        /// </summary>
        public bool Enabled { get; set; } = false;
    }
}
