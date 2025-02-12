namespace NET.Starter.Shared.Attributes
{
    /// <summary>
    /// Custom attribute used to specify the database schema for a class.
    /// This attribute is typically applied to classes that represent database entities
    /// to associate them with a specific schema.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="DatabaseSchemaAttribute"/> class.
    /// </remarks>
    /// <param name="schema">The name of the database schema associated with the class.</param>
    [AttributeUsage(AttributeTargets.Class)]
    public class DatabaseSchemaAttribute(string schema) : Attribute
    {
        /// <summary>
        /// Gets the database schema name associated with the class.
        /// </summary>
        public string Schema { get; } = schema;
    }
}
