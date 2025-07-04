using NET.Starter.Core.Services.Security.CustomModels;

namespace NET.Starter.Core.Services.Security.Dtos
{
    public class LoginDto : TokenResult
    {
        public string FullName { get; set; } = string.Empty;

        public string PictureUrl { get; set; } = string.Empty;
    }
}
