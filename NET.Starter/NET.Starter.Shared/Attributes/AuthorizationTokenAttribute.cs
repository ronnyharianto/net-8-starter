namespace NET.Starter.Shared.Attributes
{
    /// <summary>
    /// Custom attribute used to specify required authorization tokens for a method.
    /// This attribute is typically applied to controller actions to enforce token-based authorization.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="AuthorizationTokenAttribute"/> class.
    /// </remarks>
    /// <param name="token">The tokens required for the method.</param>
    [AttributeUsage(AttributeTargets.Method)] // Restricts the usage of this attribute to methods only.
    public class AuthorizationTokenAttribute(params string[] token) : Attribute
    {
        /// <summary>
        /// Gets the list of authorization tokens required to access the method.
        /// </summary>
        public string[] Token { get; } = token;
    }
}