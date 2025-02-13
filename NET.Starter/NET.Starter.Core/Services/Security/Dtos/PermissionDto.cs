namespace NET.Starter.Core.Services.Security.Dtos
{
    /// <summary>
    /// Represents a Data Transfer Object (DTO) for a permission entity.
    /// </summary>
    public class PermissionDto
    {
        /// <summary>
        /// Gets or sets the unique identifier for the permission.
        /// </summary>
        public Guid PermissionId { get; set; }

        /// <summary>
        /// Gets or sets the code that uniquely identifies the permission.
        /// </summary>
        /// <remarks>
        /// This property is required and cannot be null or empty.
        /// </remarks>
        public required string PermissionCode { get; set; }
    }
}
