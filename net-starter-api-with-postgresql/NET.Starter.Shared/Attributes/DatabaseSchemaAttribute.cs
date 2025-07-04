namespace NET.Starter.Shared.Attributes
{
    /// <summary>
    /// Specifies the database schema associated with a class.
    /// This attribute is typically applied to entity classes to indicate their schema.
    /// <para>Example: [DatabaseSchema("masterdata")]</para>
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class DatabaseSchemaAttribute(string schema) : Attribute
    {
        /// <summary>
        /// Gets the database schema name associated with the class.
        /// </summary>
        public string Schema { get; } = schema;
    }
}