namespace NET.Starter.Shared.Objects.Dtos
{
    /// <summary>
    /// Represents a time zone with its identifier and display name.
    /// </summary>
    public class TimeZoneDto
    {
        /// <summary>
        /// Gets or sets the unique identifier of the time zone (e.g., "Asia/Jakarta").
        /// </summary>
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the display name of the time zone (e.g., "(UTC+07:00) Western Indonesia Time (Jakarta)").
        /// </summary>
        public string DisplayName { get; set; } = string.Empty;
    }
}