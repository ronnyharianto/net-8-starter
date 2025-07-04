using AutoMapper;
using NET.Starter.Core.Services.Security.CustomModels;
using NET.Starter.Core.Services.Security.Dtos;

namespace NET.Starter.Core.Services.Security.Configurations
{
    internal class AccountMapperConfiguration : Profile
    {
        public AccountMapperConfiguration()
        {
            #region Transform Custom Model into Dto

            CreateMap<TokenResult, LoginDto>()
                .AfterMap((src, dest, context) =>
                {
                    if (context.TryGetItems(out var items))
                    {
                        if (items.TryGetValue("FullName", out var objFullName) && objFullName is string fullName)
                            dest.FullName = fullName;

                        if (items.TryGetValue("PictureUrl", out var objPictureUrl) && objPictureUrl is string pictureUrl)
                            dest.PictureUrl = pictureUrl;
                    }
                });

            #endregion
        }
    }
}
