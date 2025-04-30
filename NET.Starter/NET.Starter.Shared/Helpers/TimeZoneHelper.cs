using NET.Starter.Shared.Objects.Configs;

namespace NET.Starter.Shared.Helpers
{
    /// <summary>
    /// Provides methods to convert datetime values between UTC and a configured system timezone.
    /// </summary>
    public static class TimeZoneHelper
    {
        private static TimeZoneConfig? _timeZoneConfig;

        /// <summary>
        /// Initializes the timezone helper with configuration settings.
        /// This method should be called once during application startup.
        /// </summary>
        /// <param name="config">The timezone configuration.</param>
        public static void Initialize(TimeZoneConfig config)
        {
            _timeZoneConfig = config;
        }

        /// <summary>
        /// Converts a UTC datetime value to the configured system timezone.
        /// </summary>
        /// <param name="utcDateTime">The UTC datetime value.</param>
        /// <param name="timezoneId">The ID of the system timezone.</param>
        /// <returns>The converted datetime value.</returns>
        public static DateTime ConvertToTimezoneId(DateTime utcDateTime, string timezoneId)
        {
            if (_timeZoneConfig == null || !_timeZoneConfig.Enabled)
                return utcDateTime;

            try
            {
                var timezone = TimeZoneInfo.FindSystemTimeZoneById(timezoneId);
                return TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, timezone);
            }
            catch (TimeZoneNotFoundException)
            {
                throw new TimeZoneNotFoundException("The specified timezone ID is not found.");
            }
        }

        /// <summary>
        /// Converts a UTC datetime value to the default system timezone from configuration.
        /// </summary>
        /// <param name="utcDateTime">The UTC datetime value.</param>
        /// <returns>The converted datetime value.</returns>
        public static DateTime ConvertToTimezoneId(DateTime utcDateTime)
        {
            return _timeZoneConfig == null
                ? utcDateTime
                : ConvertToTimezoneId(utcDateTime, _timeZoneConfig.SystemTimeZone);
        }

        /// <summary>
        /// Converts a local datetime to UTC using the specified timezone ID.
        /// </summary>
        /// <param name="localDateTime">The local datetime value.</param>
        /// <param name="timezoneId">The ID of the system timezone.</param>
        /// <returns>The UTC datetime value.</returns>
        public static DateTime ConvertToUtc(DateTime localDateTime, string timezoneId)
        {
            if (_timeZoneConfig == null || !_timeZoneConfig.Enabled)
                return localDateTime;

            try
            {
                var timezone = TimeZoneInfo.FindSystemTimeZoneById(timezoneId);
                return TimeZoneInfo.ConvertTimeToUtc(localDateTime, timezone);
            }
            catch (TimeZoneNotFoundException)
            {
                throw new TimeZoneNotFoundException("The specified timezone ID is not found.");
            }
        }

        /// <summary>
        /// Converts a local datetime to UTC using the default system timezone from configuration.
        /// </summary>
        /// <param name="localDateTime">The local datetime value.</param>
        /// <returns>The UTC datetime value.</returns>
        public static DateTime ConvertToUtc(DateTime localDateTime)
        {
            return _timeZoneConfig == null
                ? localDateTime
                : ConvertToUtc(localDateTime, _timeZoneConfig.SystemTimeZone);
        }
    }
}
