using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("NET.Starter.API")]
[assembly: InternalsVisibleTo("NET.Starter.API.Core")]
[assembly: InternalsVisibleTo("NET.Starter.API.DataAccess")]

namespace NET.Starter.API.Shared.Objects
{
    internal class CurrentUserAccessor
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public IEnumerable<string>? Permissions { get; set; }
    }
}
