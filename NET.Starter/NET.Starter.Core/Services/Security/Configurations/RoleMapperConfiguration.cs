using AutoMapper;
using NET.Starter.Core.Services.Security.Dtos;
using NET.Starter.Core.Services.Security.Inputs;
using NET.Starter.DataAccess.SqlServer.Models.Security;

namespace NET.Starter.Core.Services.Security.Configurations
{
    /// <summary>
    /// Defines mapping configurations for Role-related objects.
    /// </summary>
    /// <remarks>
    /// This configuration includes mappings for transforming Role entities into RoleDto objects 
    /// and RoleInput objects into Role entities.
    /// </remarks>
    internal class RoleMapperConfiguration : Profile
    {
        public RoleMapperConfiguration()
        {
            #region Transform Entity into Dto

            CreateMap<Role, RoleDto>()
                .ForMember(dest => dest.RoleId, opt => opt.MapFrom(d => d.Id));

            #endregion

            #region Transform Input into Entity

            CreateMap<RoleInput, Role>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
            
            #endregion
        }
    }
}
