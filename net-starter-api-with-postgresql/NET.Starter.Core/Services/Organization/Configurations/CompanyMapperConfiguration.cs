using AutoMapper;
using NET.Starter.Core.Services.Organization.Dtos;
using NET.Starter.Core.Services.Organization.Inputs;
using NET.Starter.DataAccess.Models.Organization;

namespace NET.Starter.Core.Services.Organization.Configurations
{
    internal class CompanyMapperConfiguration : Profile
    {
        public CompanyMapperConfiguration()
        {
            #region Transform Entity into Dto

            CreateMap<Company, CompanyDto>()
                .ForMember(dest => dest.CompanyId, opt => opt.MapFrom(d => d.Id));

            #endregion

            #region Transform Input into Entity

            CreateMap<CompanyInput, Company>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Code, opt => opt.Ignore())
                .AfterMap((src, dest, context) =>
                {
                    bool isCreate = context.TryGetItems(out var items) && items.TryGetValue("IsCreate", out var value) && value is bool b && b;

                    if (isCreate)
                    {
                        dest.Code = src.Code;
                    }
                });

            #endregion
        }
    }
}
