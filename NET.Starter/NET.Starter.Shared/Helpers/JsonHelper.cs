using Newtonsoft.Json;

namespace NET.Starter.Shared.Helpers
{
    public static class JsonHelper
    {
        private static readonly JsonSerializerSettings _settings = new()
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore, // Prevents infinite loops when serializing objects with circular references.
            NullValueHandling = NullValueHandling.Ignore, // Omits properties with null values from the JSON output to reduce size.
            Formatting = Formatting.None, // Produces a compact JSON format; use Formatting.Indented for human readability.
            DateFormatHandling = DateFormatHandling.IsoDateFormat, // Ensures dates are serialized in the ISO 8601 format (YYYY-MM-DDTHH:mm:ssZ).
            DateTimeZoneHandling = DateTimeZoneHandling.RoundtripKind, // Preserves the DateTime Kind (UTC, Local, or Unspecified) during serialization and deserialization.
            DefaultValueHandling = DefaultValueHandling.Include, // Ensures all properties, including those with default values, are included in the JSON output.
            MissingMemberHandling = MissingMemberHandling.Ignore, // Prevents exceptions when deserializing JSON that contains unknown properties.
            Error = (sender, args) => // Suppresses deserialization errors by marking them as handled.
            {
                args.ErrorContext.Handled = true;
            }
        };

        /// <summary>
        /// Converts an object to its JSON string representation.
        /// </summary>
        /// <param name="data">The object to serialize.</param>
        /// <param name="indented">Whether to format the JSON output for readability.</param>
        /// <returns>The JSON string representation of the object.</returns>
        public static string SerializeObject(object? data, bool indented = false) =>
            JsonConvert.SerializeObject(data, indented ? Formatting.Indented : Formatting.None, _settings);

        /// <summary>
        /// Converts a JSON string to an object of the specified type.
        /// </summary>
        /// <typeparam name="T">The type of the object to deserialize.</typeparam>
        /// <param name="jsonString">The JSON string to deserialize.</param>
        /// <returns>The deserialized object, or null if the conversion fails.</returns>
        public static T? DeserializeObject<T>(string jsonString) =>
            JsonConvert.DeserializeObject<T>(jsonString, _settings);
    }
}
