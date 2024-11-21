namespace NET.Starter.Shared.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class DatabaseSchemaAttribute(string schema) : Attribute
    {
        public string Schema { get; } = schema;
    }
}
