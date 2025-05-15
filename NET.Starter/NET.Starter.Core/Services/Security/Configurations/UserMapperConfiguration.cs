using AutoMapper;
using NET.Starter.Core.Services.Security.Dtos;
using NET.Starter.Core.Services.Security.Inputs;
using NET.Starter.DataAccess.SqlServer.Models.Security;

namespace NET.Starter.Core.Services.Security.Configurations
{
    internal class UserMapperConfiguration : Profile
    {
        public UserMapperConfiguration()
        {
            #region Transform Entity into Dto

            CreateMap<User, UserDto>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(d => d.Id));

            CreateMap<UserCompany, UserCompanyDto>()
                .ForMember(dest => dest.UserCompanyId, opt => opt.MapFrom(d => d.Id))
                .ForMember(dest => dest.Roles, opt => opt.MapFrom(d => d.UserCompanyRoles.Select(d => d.Role)));

            #endregion

            #region Transform Input into Entity

            CreateMap<UserInput, User>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Password, opt => opt.Ignore())
                .ForMember(dest => dest.UserCompanies, opt => opt.Ignore())
                .AfterMap((src, dest, context) => 
                {
                    bool isCreate = context.TryGetItems(out var items) && items.TryGetValue("IsCreate", out var value) && value is bool b && b;

                    if (isCreate)
                    {
                        dest.UserCompanies = src.UserCompanies.Select(d => new UserCompany { 
                            UserId = dest.Id, 
                            CompanyId = d.CompanyId, 
                            UserCompanyRoles = d.RoleIds.Select(d => new UserCompanyRole
                            {
                                RoleId = d
                            }).ToHashSet()
                        }).ToHashSet();

                        dest.Password = src.Password;
                    }
                    else
                    {
                        // Only update password if it's not empty

                        if (!string.IsNullOrEmpty(src.Password))
                        {
                            dest.Password = src.Password;
                        }
                    }
                });

            CreateMap<UserCompanyInput, UserCompany>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.MapFrom((src, dest, destMember, context) =>
                {
                    try
                    {
                        return context.Items.TryGetValue("UserId", out object? value) && value is Guid ? value : dest.UserId;
                    }
                    catch 
                    {
                        return dest.UserId;
                    }
                }))
                .ForMember(dest => dest.UserCompanyRoles, opt => opt.Ignore())
                .AfterMap((src, dest, context) =>
                {
                    bool isCreate = context.TryGetItems(out var items) && items.TryGetValue("IsCreate", out var value) && value is bool b && b;

                    if (isCreate)
                    {
                        dest.UserCompanyRoles = src.RoleIds.Select(d => new UserCompanyRole
                        {
                            RoleId = d
                        }).ToHashSet();
                    }
                });

            #endregion
        }
    }
}
