namespace NET.Starter.API.Core.Services.Security.Dtos
{
    public class TokenDto
    {
        public string AccessToken { get; set; } = string.Empty;
        public DateTime ExpiredAt { get; set; }
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime SessionExpiredAt { get; set; }
    }
}
