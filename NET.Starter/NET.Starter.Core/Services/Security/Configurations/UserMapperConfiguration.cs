using AutoMapper;
using NET.Starter.Core.Services.Security.Dtos;
using NET.Starter.Core.Services.Security.Inputs;
using NET.Starter.DataAccess.SqlServer.Models.Security;

namespace NET.Starter.Core.Services.Security.Configurations
{
    /// <summary>
    /// Defines mapping configurations for User-related objects.
    /// </summary>
    /// <remarks>
    /// This configuration includes mappings for transforming User entities into UserDto objects 
    /// and UserInput objects into User entities.
    /// </remarks>
    internal class UserMapperConfiguration : Profile
    {
        public UserMapperConfiguration()
        {
            #region Transform Entity into Dto

            CreateMap<User, UserDto>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(d => d.Id))
                .ForMember(dest => dest.Roles, opt => opt.MapFrom(d => d.UserRoles.Select(d => d.Role)));

            #endregion

            #region Transform Input into Entity

            CreateMap<UserInput, User>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .AfterMap((src, dest, context) => 
                {
                    bool isCreate = context.TryGetItems(out var items) && items.TryGetValue("IsCreate", out var value) && value is bool b && b;

                    if (isCreate)
                    {
                        dest.UserRoles = src.RoleIds.Select(d => new UserRole { UserId = dest.Id, RoleId = d }).ToHashSet();
                    }
                });
            
            #endregion
        }
    }
}
