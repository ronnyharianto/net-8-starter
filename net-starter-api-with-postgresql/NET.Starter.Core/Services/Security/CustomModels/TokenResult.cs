namespace NET.Starter.Core.Services.Security.CustomModels
{
    public class TokenResult
    {
        public required string Token { get; set; }

        public required DateTime TokenExpiresAt { get; set; }

        public required string Refresh { get; set; }

        public required DateTime RefreshExpiresAt { get; set; }
    }
}
