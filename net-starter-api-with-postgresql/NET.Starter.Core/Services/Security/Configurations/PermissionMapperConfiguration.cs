using AutoMapper;
using NET.Starter.Core.Services.Security.Dtos;
using NET.Starter.DataAccess.Models.Security;

namespace NET.Starter.Core.Services.Security.Configurations
{
    internal class PermissionMapperConfiguration : Profile
    {
        public PermissionMapperConfiguration()
        {
            #region Transform Entity into Dto

            CreateMap<Permission, PermissionDto>()
                .ForMember(dest => dest.PermissionId, opt => opt.MapFrom(d => d.Id));

            #endregion
        }
    }
}