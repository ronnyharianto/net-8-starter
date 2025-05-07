namespace NET.Starter.Shared.Objects
{
    /// <summary>
    /// Represents the currently authenticated user.
    /// This class stores identity-related information extracted from the authentication token.
    /// </summary>
    public class CurrentUserAccessor
    {
        /// <summary>
        /// Unique identifier of the user.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Full name of the authenticated user.
        /// Example: "John Doe".
        /// </summary>
        public string FullName { get; set; } = string.Empty;

        /// <summary>
        /// Email address of the user.
        /// Example: "john.doe@example.com".
        /// </summary>
        public string EmailAddress { get; set; } = string.Empty;

        /// <summary>
        /// Unique identifier of the company currently accessed by the user.
        /// </summary>
        public Guid CompanyId { get; set; }

        /// <summary>
        /// Time zone of the user.
        /// Example: "UTC".
        /// </summary>
        public string UserTimeZone { get; set; } = "UTC";

        /// <summary>
        /// List of permissions granted to the user.
        /// Example for general purpose: ["RefreshToken", "MyPermission", "IamAdministrator", etc].
        /// Example for specific menu: ["MasterData.Company.Menu", "MasterData.Company.View", "MasterData.Company.Create", etc].
        /// </summary>
        public IEnumerable<string>? Permissions { get; set; }
    }
}
