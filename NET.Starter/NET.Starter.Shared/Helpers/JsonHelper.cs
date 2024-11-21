using Newtonsoft.Json;

namespace NET.Starter.Shared.Helpers
{
    public static class JsonHelper
    {
        private static readonly JsonSerializerSettings settings = new()
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };

        /// <summary>
        /// Serializes the specified object to a JSON string.
        /// </summary>
        public static string SerializeObject(object? data) => JsonConvert.SerializeObject(data, Formatting.None, settings);

        /// <summary>
        /// Deserializes the specified JSON string to an object of the specified type.
        /// </summary>
        public static T? DeserializeObject<T>(string jsonString) => JsonConvert.DeserializeObject<T>(jsonString, settings);
    }
}
