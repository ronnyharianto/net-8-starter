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
        public const string TypeCode = "permissions";

        /// <summary>
        /// Key used for accessing the endpoint to refresh a JWT token.
        /// </summary>
        public const string RefreshToken = "RefreshToken";

        /// <summary>
        /// Nested class defining constants related to identity and roles.
        /// Used to identify user roles for authorization or role-based access control (RBAC).
        /// </summary>
        public static class Identity
        {
            public const string Admin = "IamAdministrator";
        }

        public static class Security
        {
            public static class Permission
            {
                public const string View = "Security.Permission.View";
            }

            public static class Role
            {
                public const string Menu = "Security.Role.Menu";
                public const string View = "Security.Role.View";
                public const string Create = "Security.Role.Create";
                public const string Update = "Security.Role.Update";
                public const string Delete = "Security.Role.Delete";
            }

            public static class User
            {
                public const string Menu = "Security.User.Menu";
                public const string View = "Security.User.View";
                public const string Create = "Security.User.Create";
                public const string Update = "Security.User.Update";
                public const string Delete = "Security.User.Delete";
            }
        }

        public static class Organization
        {
            public static class Company
            {
                public const string Menu = "Organization.Company.Menu";
                public const string View = "Organization.Company.View";
                public const string Create = "Organization.Company.Create";
                public const string Update = "Organization.Company.Update";
                public const string Delete = "Organization.Company.Delete";
            }

            public static class Branch
            {
                public const string Menu = "Organization.Branch.Menu";
                public const string View = "Organization.Branch.View";
                public const string Create = "Organization.Branch.Create";
                public const string Update = "Organization.Branch.Update";
                public const string Delete = "Organization.Branch.Delete";
            }
        }
    }
}