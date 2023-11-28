using AutoMapper;
using POS.Data.Dto;
using POS.Data;
using POS.MediatR.Banner.Command;

namespace POS.API.Helpers.Mapping
{
    public class BannerProfile : Profile
    {
        public BannerProfile()
        {
            CreateMap<Banner, BannerDto>().ReverseMap();
            CreateMap<AddBannerCommand, Banner>();
        }
    }
}
