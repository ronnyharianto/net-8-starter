namespace NET.Starter.Shared.Attributes
{
    /// <summary>
    /// Custom attribute used to mark a method as a mutation operation.
    /// Typically applied in scenarios where the method represents a change or modification 
    /// to the underlying data in transactional operations.
    /// </summary>
    /// <remarks>
    /// This attribute can be used for documentation purposes or runtime processing 
    /// to distinguish mutation operations from other types of methods.
    /// <para>
    /// Initializes a new instance of the <see cref="MutationAttribute"/> class.
    /// </para>
    /// </remarks>
    [AttributeUsage(AttributeTargets.Method)]
    public class MutationAttribute() : Attribute
    { }
}
