namespace NET.Starter.Shared.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public class MapFromAttribute<T>(T target) : Attribute
        where T : Enum
    {
        public T Target { get; } = target;
    }
}
