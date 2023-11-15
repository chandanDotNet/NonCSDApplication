using AutoMapper;
using POS.Data;
using POS.Data.Dto;
using POS.MediatR.Brand.Command;

namespace POS.API.Helpers.Mapping
{
    public class BrandProfile : Profile
    {
        public BrandProfile()
        {
            CreateMap<Brand, BrandDto>().ReverseMap();
            CreateMap<AddBrandCommand, Brand>();
        }
    }
}
