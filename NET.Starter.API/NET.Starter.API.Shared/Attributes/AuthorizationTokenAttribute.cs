namespace NET.Starter.API.Shared.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class AuthorizationTokenAttribute(params string[] token) : Attribute
    {
        public string[] Token { get; } = token;
    }
}
