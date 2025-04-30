using NET.Starter.Core.Services.Security.CustomModels;

namespace NET.Starter.Core.Services.Security.Dtos
{
    /// <summary>
    /// Represents a Data Transfer Object (DTO) for login information.
    /// This includes token details, user-specific data, and assigned permissions.
    /// </summary>
    public class LoginDto : TokenResult
    {
        /// <summary>
        /// Gets or sets the full name of the authenticated user.
        /// </summary>
        public string FullName { get; set; } = string.Empty;
    }
}
