namespace NET.Starter.Core.Services.Security.Dtos
{
    /// <summary>
    /// Represents a Data Transfer Object (DTO) for a role entity.
    /// </summary>
    public class RoleDto
    {
        /// <summary>
        /// Gets or sets the unique identifier for the role.
        /// </summary>
        public Guid RoleId { get; set; }

        /// <summary>
        /// Gets or sets the code that uniquely identifies the role.
        /// </summary>
        /// <remarks>
        /// This property is required and cannot be null or empty.
        /// </remarks>
        public required string RoleCode { get; set; }

        /// <summary>
        /// Gets or sets the collection of permissions associated with the role.
        /// </summary>
        /// <remarks>
        /// The collection is initialized as an empty array by default.
        /// Each permission is represented as a <see cref="PermissionDto"/>.
        /// </remarks>
        public ICollection<PermissionDto> Permissions { get; set; } = [];
    }
}
