namespace NET.Starter.Shared.Constants
{
    /// <summary>
    /// Defines constants related to permission handling and authentication.
    /// These constants are typically used for JWT token claims and API endpoint access.
    /// </summary>
    public static class PermissionConstants
    {
        /// <summary>
        /// Key used in JWT tokens to represent the permission type.
        /// </summary>
        public const string TypeCode = "Permission";

        /// <summary>
        /// Key used for accessing the endpoint to refresh a JWT token.
        /// </summary>
        public const string RefreshToken = "RefreshToken";

        /// <summary>
        /// Key used for accessing the endpoint to retrieve the current user's permissions.
        /// </summary>
        public const string MyPermission = "MyPermission";

        /// <summary>
        /// Nested class defining constants related to identity and roles.
        /// Used to identify user roles for authorization or role-based access control (RBAC).
        /// </summary>
        public static class Identity
        {
            /// <summary>
            /// Represents the role or identifier for an Administrator user.
            /// This constant can be used to assign or check admin privileges.
            /// </summary>
            public const string Admin = "IamAdministrator";
        }
    }
}