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
            public const string Admin = "IamAdministrator";
        }

        /// <summary>
        /// Nested class defining constants related to security features.
        /// Used for security-related operations such as authentication and authorization.
        /// </summary>
        public static class Security
        {
            /// <summary>
            /// Represents the permission type for viewing security features.
            /// </summary>
            public static class Permission
            {
                public const string View = "Security.Permission.View";
            }

            /// <summary>
            /// Nested class defining constants related to roles in the security module.
            /// Used for role-based access control (RBAC).
            /// </summary>
            public static class Role
            {
                public const string Menu = "Security.Role.Menu";
                public const string View = "Security.Role.View";
                public const string Create = "Security.Role.Create";
                public const string Update = "Security.Role.Update";
                public const string Delete = "Security.Role.Delete";
            }
        }
    }
}