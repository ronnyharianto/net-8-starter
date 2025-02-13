namespace NET.Starter.Core.Services.Security.Inputs
{
    /// <summary>
    /// Represents the input model for creating or updating a role.
    /// </summary>
    public class RoleInput
    {
        /// <summary>
        /// Gets or sets the unique code for the role.
        /// </summary>
        /// <remarks>
        /// This property is required and must be provided when creating or updating a role.
        /// </remarks>
        public required string RoleCode { get; set; }

        /// <summary>
        /// Gets or sets the collection of permission IDs associated with the role.
        /// </summary>
        /// <remarks>
        /// The collection is initialized as an empty list by default.
        /// Each GUID in the collection represents a unique permission to be assigned to the role.
        /// </remarks>
        public IEnumerable<Guid> PermissionIds { get; set; } = [];
    }
}
