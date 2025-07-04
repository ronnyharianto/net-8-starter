using NET.Starter.Shared.Objects.Configs;
using NET.Starter.Shared.Objects.Dtos;
using Serilog;

namespace NET.Starter.Shared.Helpers
{
    /// <summary>
    /// Provides methods to convert datetime values between UTC and a configured system timezone.
    /// Use this when your application stores time in UTC and needs to display local time based on user, branch, or company settings.
    /// </summary>
    public static class TimeZoneHelper
    {
        private static TimeZoneConfig? _timeZoneConfig;

        /// <summary>
        /// Initializes the timezone configuration.
        /// Should be called once during application startup.
        /// </summary>
        /// <param name="config">The timezone configuration object.</param>
        internal static void Initialize(TimeZoneConfig config)
        {
            _timeZoneConfig = config;
            Log.Information("Timezone helper enabled with system timezone: {TimeZoneId}", config.SystemTimeZone);
        }

        /// <summary>
        /// Converts a UTC datetime to the specified timezone Id.
        /// </summary>
        /// <param name="utcDateTime">The UTC datetime value.</param>
        /// <param name="timezoneId">The target timezone Id.</param>
        /// <returns>The datetime converted to the specified timezone, or original UTC value if disabled or error.</returns>
        public static DateTime ConvertToTimezoneId(DateTime utcDateTime, string timezoneId)
        {
            if (_timeZoneConfig?.Enabled != true)
                return utcDateTime;

            try
            {
                var timezone = TimeZoneInfo.FindSystemTimeZoneById(timezoneId);
                return TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, timezone);
            }
            catch (TimeZoneNotFoundException)
            {
                Log.Warning("ConvertToTimezoneId failed: Time zone ID '{TimeZoneId}' not found.", timezoneId);
                throw new TimeZoneNotFoundException("The specified timezone ID is not found.");
            }
        }

        /// <summary>
        /// Converts a UTC datetime to the default system timezone from configuration.
        /// </summary>
        /// <param name="utcDateTime">The UTC datetime value.</param>
        /// <returns>The datetime converted to system timezone, or UTC if config is not initialized or disabled.</returns>
        public static DateTime ConvertToTimezoneId(DateTime utcDateTime)
        {
            return _timeZoneConfig == null
                ? utcDateTime
                : ConvertToTimezoneId(utcDateTime, _timeZoneConfig.SystemTimeZone);
        }

        /// <summary>
        /// Converts a local datetime to UTC based on the specified timezone ID.
        /// </summary>
        /// <param name="localDateTime">The local datetime value.</param>
        /// <param name="timezoneId">The timezone ID the datetime is based on.</param>
        /// <returns>The UTC equivalent of the local datetime.</returns>
        public static DateTime ConvertToUtc(DateTime localDateTime, string timezoneId)
        {
            if (_timeZoneConfig?.Enabled != true)
                return localDateTime;

            try
            {
                var timezone = TimeZoneInfo.FindSystemTimeZoneById(timezoneId);
                return TimeZoneInfo.ConvertTimeToUtc(localDateTime, timezone);
            }
            catch (TimeZoneNotFoundException)
            {
                Log.Warning("ConvertToUtc failed: Time zone ID '{TimeZoneId}' not found.", timezoneId);
                throw new TimeZoneNotFoundException("The specified timezone ID is not found.");
            }
        }

        /// <summary>
        /// Converts a local datetime to UTC using the default system timezone from configuration.
        /// </summary>
        /// <param name="localDateTime">The local datetime value.</param>
        /// <returns>The UTC equivalent, or input value if config is missing or disabled.</returns>
        public static DateTime ConvertToUtc(DateTime localDateTime)
        {
            return _timeZoneConfig == null
                ? localDateTime
                : ConvertToUtc(localDateTime, _timeZoneConfig.SystemTimeZone);
        }

        /// <summary>
        /// Retrieves a list of available system time zones.
        /// </summary>
        /// <returns>A collection of <see cref="TimeZoneDto"/> objects with ID and display name.</returns>
        public static IEnumerable<TimeZoneDto> RetrieveTimezones()
        {
            return TimeZoneInfo.GetSystemTimeZones().Select(tz => new TimeZoneDto
            {
                Id = tz.Id,
                DisplayName = tz.DisplayName
            });
        }

        /// <summary>
        /// Validates if the specified timezone ID exists on the current system.
        /// </summary>
        /// <param name="timeZoneId">The timezone ID to validate.</param>
        /// <returns><c>true</c> if valid; otherwise, <c>false</c>.</returns>
        public static bool ValidateTimeZoneId(string timeZoneId)
        {
            var isValid = TimeZoneInfo.TryFindSystemTimeZoneById(timeZoneId, out _);

            if (!isValid)
            {
                Log.Warning("Timezone ID '{TimeZoneId}' is invalid or not found on the system.", timeZoneId);
            }

            return isValid;
        }
    }
}
