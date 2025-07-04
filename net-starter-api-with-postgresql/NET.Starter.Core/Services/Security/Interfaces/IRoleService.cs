using NET.Starter.Core.Services.Security.Dtos;
using NET.Starter.Core.Services.Security.Inputs;
using NET.Starter.Shared.Objects.Dtos;
using NET.Starter.Shared.Objects.Inputs;

namespace NET.Starter.Core.Services.Security.Interfaces
{
    public interface IRoleService
    {
        Task<ObjectDto<IEnumerable<RoleDto>>> RetrieveRolesAsync();

        Task<PagingDto<RoleDto>> RetrieveRolesPagingAsync(PagingSearchInputBase input);

        Task<ObjectDto<RoleDto>> RetrieveRoleByIdAsync(Guid roleId);

        Task<BaseDto> CreateRoleAsync(RoleInput input);

        Task<BaseDto> UpdateRoleAsync(Guid roleId, RoleInput input);

        Task<BaseDto> DeleteRoleAsync(Guid roleId);
    }
}
