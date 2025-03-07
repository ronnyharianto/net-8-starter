namespace NET.Starter.Core.Services.Security.Dtos
{
    public class UserDto
    {
        /// <summary>
        /// Gets or sets the unique identifier of a specific user.
        /// </summary>
        public Guid UserId { get; set; }

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
        /// Gets or sets the full name of the user.
        /// This property is required.
        /// </summary>
        public required string Fullname { get; set; }

        /// <summary>
        /// Gets or sets the roles associated with a specific user.
        /// </summary>
        /// <remarks>
        /// The collection is initialized as an empty array by default.
        /// Each role is represented as a <see cref="RoleDto"/>.
        /// </remarks>
        public IEnumerable<RoleDto> Roles { get; set; } = [];
    }
}
