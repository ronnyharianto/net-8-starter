namespace NET.Starter.Shared.Attributes
{
    /// <summary>
    /// Maps a field to a specific value from an enum of type <typeparamref name="T"/>.
    /// Use this attribute to associate a field with a corresponding enum value,
    /// which can help with mapping or configuration scenarios.
    /// <para>
    /// Example:
    /// <code>
    /// enum Dietary
    /// { 
    ///     Herbivore, 
    ///     Carnivore,
    ///     Omnivore
    /// }
    ///
    /// enum Animal
    /// {
    ///     [MapFrom&lt;Dietary&gt;(Dietary.Herbivore)]
    ///     Rabbit,
    ///     [MapFrom&lt;Dietary&gt;(Dietary.Herbivore)]
    ///     Giraffe,
    ///     [MapFrom&lt;Dietary&gt;(Dietary.Carnivore)]
    ///     Tiger,
    ///     [MapFrom&lt;Dietary&gt;(Dietary.Carnivore)]
    ///     Lion
    /// }
    /// </code>
    /// </para>
    /// </summary>
    /// <typeparam name="T">The enum type that the field maps from.</typeparam>
    [AttributeUsage(AttributeTargets.Field)]
    public class MapFromAttribute<T>(T target) : Attribute
        where T : Enum
    {
        /// <summary>
        /// Gets the enum value that the field maps from.
        /// </summary>
        public T Target { get; } = target;
    }
}
