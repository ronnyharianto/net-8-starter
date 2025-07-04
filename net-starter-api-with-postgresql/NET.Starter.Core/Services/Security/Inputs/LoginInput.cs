namespace NET.Starter.Core.Services.Security.Inputs
{
    public class LoginInput
    {
        /// <summary>
        /// Identifier of the user (e.g., username or email).
        /// </summary>
        public required string UserIdentifier { get; set; }

        public required string Password { get; set; }
    }
}
