using AutoMapper;
using NET.Starter.Core.Services.Security.Dtos;
using NET.Starter.Core.Services.Security.Inputs;
using NET.Starter.DataAccess.Models.Security;

namespace NET.Starter.Core.Services.Security.Configurations
{
    internal class RoleMapperConfiguration : Profile
    {
        public RoleMapperConfiguration()
        {
            #region Transform Entity into Dto

            CreateMap<Role, RoleDto>()
                .ForMember(dest => dest.RoleId, opt => opt.MapFrom(d => d.Id))
                .ForMember(dest => dest.Permissions, opt => opt.MapFrom(d => d.RolePermissions.Select(d => d.Permission)));

            #endregion

            #region Transform Input into Entity

            CreateMap<RoleInput, Role>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .AfterMap((src, dest, context) =>
                {
                    bool isCreate = context.TryGetItems(out var items) && items.TryGetValue("IsCreate", out var value) && value is bool b && b;

                    if (isCreate)
                    {
                        dest.RolePermissions = src.PermissionIds.Select(d => new RolePermission { RoleId = dest.Id, PermissionId = d }).ToHashSet();
                    }
                });

            #endregion
        }
    }
}
