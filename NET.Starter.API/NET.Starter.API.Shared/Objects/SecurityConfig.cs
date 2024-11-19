namespace NET.Starter.API.Shared.Objects
{
    public class SecurityConfig
    {
        public int MaximumLoginRetry { get; set; }
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public string SecretKey { get; set; } = string.Empty;
        /// <summary>
        /// Value In Minutes
        /// </summary>
        public int TokenExpired { get; set; }

        /// <summary>
        /// Value In Hours
        /// </summary>
        public int SessionExpired { get; set; }
    }
}
