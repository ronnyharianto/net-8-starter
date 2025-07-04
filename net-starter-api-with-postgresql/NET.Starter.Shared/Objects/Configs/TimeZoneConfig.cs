namespace NET.Starter.Shared.Objects.Configs
{
    /// <summary>
    /// Represents application-level configuration settings for timezone handling.
    /// </summary>
    public class TimeZoneConfig
    {
        /// <summary>
        /// The timezone used by the backend system for datetime conversions.
        /// Typically used to convert UTC timestamps to a local timezone for display.
        /// Default value: "UTC".
        /// </summary>
        public string SystemTimeZone { get; set; } = "UTC";

        /// <summary>
        /// Indicates whether timezone conversion is enabled.
        /// When <c>false</c>, datetime values are returned as UTC without conversion.
        /// <para>
        /// If the frontend handles timezone conversion, set this to <c>false</c>.
        /// Best practice is to let the frontend handle timezone conversion.
        /// Default value: <c>false</c>.
        /// </para>
        /// </summary>
        public bool Enabled { get; set; } = false;
    }
}
