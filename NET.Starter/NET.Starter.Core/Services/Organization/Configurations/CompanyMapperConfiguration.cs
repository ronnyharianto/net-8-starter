using AutoMapper;
using NET.Starter.Core.Services.Organization.Dtos;
using NET.Starter.DataAccess.SqlServer.Models.Organization;

namespace NET.Starter.Core.Services.Security.Configurations
{
    internal class CompanyMapperConfiguration : Profile
    {
        public CompanyMapperConfiguration()
        {
            #region Transform Entity into Dto

            CreateMap<Company, CompanyDto>()
                .ForMember(dest => dest.CompanyId, opt => opt.MapFrom(d => d.Id));

            #endregion
        }
    }
}
