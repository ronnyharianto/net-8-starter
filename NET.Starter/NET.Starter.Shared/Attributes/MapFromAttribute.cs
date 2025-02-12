namespace NET.Starter.Shared.Attributes
{
    /// <summary>
    /// Custom attribute used to map a field to a specific value from an enum.
    /// This can be useful for associating fields with corresponding enum values
    /// to facilitate mapping or configuration purposes.
    /// </summary>
    /// <typeparam name="T">The enum type that the field is mapped from.</typeparam>
    /// <remarks>
    /// Initializes a new instance of the <see cref="MapFromAttribute{T}"/> class with the specified target value.
    /// </remarks>
    /// <param name="target">The enum value that the field is mapped to.</param>
    [AttributeUsage(AttributeTargets.Field)]
    public class MapFromAttribute<T>(T target) : Attribute
        where T : Enum
    {
        /// <summary>
        /// Gets the enum value that the field is mapped from.
        /// </summary>
        public T Target { get; } = target;
    }
}
