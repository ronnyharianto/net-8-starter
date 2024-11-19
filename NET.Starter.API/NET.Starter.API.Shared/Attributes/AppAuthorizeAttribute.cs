namespace NET.Starter.API.Shared.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class AppAuthorizeAttribute(params string[] permissions) : Attribute
    {
        public string[] Permissions { get; } = permissions;
    }
}
