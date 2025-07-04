namespace NET.Starter.Shared.Objects
{
    /// <summary>
    /// Represents the identity and context of the currently authenticated user.
    /// Typically populated from authentication tokens and used throughout the request lifecycle.
    /// </summary>
    public class CurrentUserAccessor
    {
        /// <summary>
        /// Unique identifier assigned to the user.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// The user's full name, as stored in the identity provider.
        /// For example: "John Doe".
        /// </summary>
        public string FullName { get; set; } = string.Empty;

        /// <summary>
        /// The user's email address.
        /// For example: "john.doe@example.com".
        /// </summary>
        public string EmailAddress { get; set; } = string.Empty;

        /// <summary>
        /// Identifier of the company or tenant the user is currently accessing.
        /// </summary>
        public Guid CompanyId { get; set; }

        /// <summary>
        /// A collection of permissions granted to the user.
        /// These may include global permissions (e.g., "RefreshToken", "IamAdministrator") 
        /// or module-specific rights (e.g., "MasterData.Company.View", "MasterData.Company.Create").
        /// </summary>
        public IEnumerable<string>? Permissions { get; set; }
    }
}
