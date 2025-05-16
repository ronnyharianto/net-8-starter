using AutoMapper;
using NET.Starter.Core.Services.Organization.Dtos;
using NET.Starter.Core.Services.Organization.Inputs;
using NET.Starter.DataAccess.SqlServer.Models.Organization;

namespace NET.Starter.Core.Services.Security.Configurations
{
    internal class BranchMapperConfiguration : Profile
    {
        public BranchMapperConfiguration()
        {
            #region Transform Entity into Dto

            CreateMap<Branch, BranchDto>()
                .ForMember(dest => dest.BranchId, opt => opt.MapFrom(d => d.Id));

            #endregion

            #region Transform Input into Entity

            CreateMap<BranchInput, Branch>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.BranchCode, opt => opt.Ignore())
                .AfterMap((src, dest, context) =>
                {
                    bool isCreate = context.TryGetItems(out var items) && items.TryGetValue("IsCreate", out var value) && value is bool b && b;

                    if (isCreate)
                    {
                        dest.BranchCode = src.BranchCode;
                    }
                });

            #endregion
        }
    }
}
