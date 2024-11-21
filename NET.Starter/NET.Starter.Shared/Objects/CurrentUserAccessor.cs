namespace NET.Starter.Shared.Objects
{
    public class CurrentUserAccessor
    {
        public Guid Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public Guid? UserFcmTokenId { get; set; }
        public IEnumerable<string>? Permissions { get; set; }
    }
}
