using AutoMapper;
using NET.Starter.Core.Services.Security.CustomModels;
using NET.Starter.Core.Services.Security.Dtos;
using NET.Starter.DataAccess.SqlServer.Models.Security;

namespace NET.Starter.Core.Services.Security.Configurations
{
    /// <summary>
    /// Defines mapping configurations for Account-related objects.
    /// </summary>
    internal class AccountMapperConfiguration : Profile
    {
        public AccountMapperConfiguration()
        {
            #region Transform Entity into Dto

            CreateMap<User, LoginDto>()
                .AfterMap((src, dest, context) =>
                {
                    if (context.TryGetItems(out var items) && items.TryGetValue("MapSpecificProperties", out object? value) && (bool)value)
                    {
                        dest.FullName = src.Fullname;
                        dest.PermissionCodes = src.UserRoles.SelectMany(d => d.Role.RolePermissions.Select(d => d.Permission.PermissionCode)).OrderBy(d => d);
                    }
                })
                .ForAllMembers(opt => opt.Ignore());

            #endregion

            #region Transform Custom Model into Dto

            CreateMap<TokenResult, LoginDto>();

            #endregion
        }
    }
}