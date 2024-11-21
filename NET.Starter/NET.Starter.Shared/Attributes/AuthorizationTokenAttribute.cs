namespace NET.Starter.Shared.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class AuthorizationTokenAttribute(params string[] token) : Attribute
    {
        public string[] Token { get; } = token;
    }
}
