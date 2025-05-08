namespace NET.Starter.Shared.Attributes
{
    /// <summary>
    /// Custom attribute used to specify the required permissions for a controller action.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class AppAuthorizeAttribute(params string[] permissions) : Attribute
    {
        /// <summary>
        /// List of permissions required to access the method.
        /// </summary>
        public string[] Permissions { get; } = permissions;
    }
}