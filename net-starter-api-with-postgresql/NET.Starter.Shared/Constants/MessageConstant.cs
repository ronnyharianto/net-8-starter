namespace NET.Starter.Shared.Constants
{
    /// <summary>
    /// General constants
    /// </summary>
    public static class MessageConstant
    {
        public static class Validation
        {
            public static string Required(string field) => $"{field} is required.";
            public static string Invalid(string field) => $"{field} is in invalid format.";
            public static string NotEmpty(string field) => $"Please add at least one {field}.";
        }
    }
}