namespace NET.Starter.Shared.Attributes
{
    /// <summary>
    /// Custom attribute used to specify the required permissions for a method.
    /// This attribute is typically applied to controller actions to enforce permission-based access control.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="AppAuthorizeAttribute"/> class.
    /// </remarks>
    /// <param name="permissions">The permissions required for the method.</param>
    [AttributeUsage(AttributeTargets.Method)] // Restricts the use of this attribute to methods only.
    public class AppAuthorizeAttribute(params string[] permissions) : Attribute
    {
        /// <summary>
        /// Gets the list of permissions required to access the method.
        /// </summary>
        public string[] Permissions { get; } = permissions;
    }
}