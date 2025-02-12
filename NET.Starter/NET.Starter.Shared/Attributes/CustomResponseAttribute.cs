namespace NET.Starter.Shared.Attributes
{
    /// <summary>
    /// Custom attribute used to indicate that a method has a custom response.
    /// This attribute can be utilized for documentation purposes or runtime processing
    /// to enforce specific response handling logic.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="CustomResponseAttribute"/> class.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Method)]
    public class CustomResponseAttribute() : Attribute
    { }
}
