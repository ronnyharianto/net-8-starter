namespace NET.Starter.Shared.Constants
{
    /// <summary>
    /// Defines constants related to permission handling and authentication.
    /// These constants are used in JWT claims, endpoint access control, and authorization checks.
    /// </summary>
    public static class PermissionConstant
    {
        /// <summary>
        /// Claim key used to identify the refresh token action.
        /// </summary>
        public const string RefreshToken = "RefreshToken";

        /// <summary>
        /// Claim key used to identify the file retrieval action.
        /// </summary>
        public const string RetrieveFileFromStorage = "RetrieveFileFromStorage";

        /// <summary>
        /// Contains identity and role-related permission constants.
        /// </summary>
        public static class Identity
        {
            /// <summary>
            /// Permission for administrative access.
            /// </summary>
            public const string IamAdministrator = "Identity.IamAdministrator";
        }

        /// <summary>
        /// Permissions related to security management (user, role, etc).
        /// </summary>
        public static class Security
        {
            /// <summary>
            /// Permissions related to role management.
            /// </summary>
            public static class Role
            {
                public const string Access = "Security.Role.Access";
                public const string Modify = "Security.Role.Modify";
                public const string Delete = "Security.Role.Delete";
            }

            /// <summary>
            /// Permissions related to user management.
            /// </summary>
            public static class User
            {
                public const string Access = "Security.User.Access";
                public const string Modify = "Security.User.Modify";
                public const string Delete = "Security.User.Delete";
            }
        }

        /// <summary>
        /// Permissions related to organization-level resources.
        /// </summary>
        public static class Organization
        {
            /// <summary>
            /// Permissions related to company-level operations.
            /// </summary>
            public static class Company
            {
                public const string Access = "Organization.Company.Access";
                public const string Modify = "Organization.Company.Modify";
                public const string Delete = "Organization.Company.Delete";
            }
        }
    }
}
