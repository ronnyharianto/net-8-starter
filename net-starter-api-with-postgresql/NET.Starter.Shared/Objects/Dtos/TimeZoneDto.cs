namespace NET.Starter.Shared.Objects.Dtos
{
    /// <summary>
    /// Represents a time zone with its unique identifier and user-friendly display name.
    /// </summary>
    public class TimeZoneDto
    {
        /// <summary>
        /// The unique identifier of the time zone, e.g., "Asia/Jakarta".
        /// </summary>
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// The human-readable display name of the time zone, e.g., "(UTC+07:00) Western Indonesia Time (Jakarta)".
        /// </summary>
        public string DisplayName { get; set; } = string.Empty;
    }
}
