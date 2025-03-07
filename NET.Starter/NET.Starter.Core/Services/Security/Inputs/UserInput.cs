namespace NET.Starter.Core.Services.Security.Inputs
{
    public class UserInput
    {
        /// <summary>
        /// Gets or sets the unique username that identifies a specific user.
        /// This property is required.
        /// </summary>
        public required string Username { get; set; }

        /// <summary>
        /// Gets or sets the email address associated with a specific user.
        /// This property is required.
        /// </summary>
        public required string EmailAddress { get; set; }

        /// <summary>
        /// Gets or sets the password associated with a specific user.
        /// This property is required.
        /// </summary>
        public required string Password { get; set; }

        /// <summary>
        /// Gets or sets the full name of the user.
        /// This property is required.
        /// </summary>
        public required string Fullname { get; set; }

        /// <summary>
        /// Gets or sets the roles associated with a specific user.
        /// </summary>
        /// <remarks>
        /// The collection is initialized as an empty list by default.
        /// Each GUID in the collection represents a unique role to be assigned to the user.
        /// </remarks>
        public IEnumerable<Guid> RoleIds { get; set; } = [];
    }
}
