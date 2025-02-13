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
            // Maps the Role entity to the RoleDto.
            // - RoleId in RoleDto is mapped from Id in the Role entity.
            CreateMap<Role, RoleDto>()
                .ForMember(dest => dest.RoleId, opt => opt.MapFrom(d => d.Id));
            #endregion

            #region Transform Input into Entity
            // Maps the RoleInput object to the Role entity.
            // - Ignores the Id property in the Role entity, as it is typically auto-generated.
            CreateMap<RoleInput, Role>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
            #endregion
        }
    }
}
