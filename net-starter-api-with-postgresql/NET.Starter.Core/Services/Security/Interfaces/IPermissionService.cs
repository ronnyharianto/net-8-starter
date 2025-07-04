using NET.Starter.Core.Services.Security.Dtos;
using NET.Starter.Shared.Objects.Dtos;

namespace NET.Starter.Core.Services.Security.Interfaces
{
    public interface IPermissionService
    {
        Task<ObjectDto<IEnumerable<PermissionDto>>> RetrievePermissionsAsync();
    }
}
