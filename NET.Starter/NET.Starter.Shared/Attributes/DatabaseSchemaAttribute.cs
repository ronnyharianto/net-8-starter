namespace NET.Starter.Shared.Attributes
{
    /// <summary>
    /// Custom attribute used to specify the database schema for a class.
    /// This attribute is typically applied to classes that represent database entities
    /// to associate them with a specific schema.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class DatabaseSchemaAttribute(string schema) : Attribute
    {
        /// <summary>
        /// Database schema name associated with the class.
        /// </summary>
        public string Schema { get; } = schema;
    }
}
