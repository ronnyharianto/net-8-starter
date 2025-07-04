namespace NET.Starter.Shared.Attributes
{
    /// <summary>
    /// Specifies one or more required permissions for accessing a controller action.
    /// Use this attribute to restrict access based on defined permission strings.
    /// <para>Example: [AppAuthorize("User.Read", "User.Write")]</para>
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class AppAuthorizeAttribute(params string[] permissions) : Attribute
    {
        /// <summary>
        /// Gets the list of permissions required to access the action.
        /// </summary>
        public string[] Permissions { get; } = permissions;
    }
}