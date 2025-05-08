namespace NET.Starter.Shared.Attributes
{
    /// <summary>
    /// Custom attribute used to map a field to a specific value from an enum.
    /// This can be useful for associating fields with corresponding enum values
    /// to facilitate mapping or configuration purposes.
    /// </summary>
    /// <typeparam name="T">The enum type that the field is mapped from.</typeparam>
    [AttributeUsage(AttributeTargets.Field)]
    public class MapFromAttribute<T>(T target) : Attribute
        where T : Enum
    {
        /// <summary>
        /// Enum value that the field is mapped from.
        /// </summary>
        public T Target { get; } = target;
    }
}
