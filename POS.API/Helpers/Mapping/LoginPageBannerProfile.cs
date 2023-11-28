using AutoMapper;
using POS.Data.Dto;
using POS.Data;
using POS.MediatR.Banner.Command;

namespace POS.API.Helpers.Mapping
{
    public class LoginPageBannerProfile : Profile
    {
        public LoginPageBannerProfile()
        {
            CreateMap<LoginPageBanner, LoginPageBannerDto>().ReverseMap();
            CreateMap<AddLoginPageBannerCommand, LoginPageBanner>();
        }
    }
}
