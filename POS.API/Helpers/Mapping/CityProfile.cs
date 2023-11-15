using AutoMapper;
using POS.Data;
using POS.Data.Dto;
using POS.MediatR.City.Commands;

namespace POS.API.Helpers.Mapping
{
    public class CityProfile : Profile
    {
        public CityProfile()
        {
            CreateMap<City, CityDto>().ReverseMap();
            CreateMap<AddCityCommand, City>();
            CreateMap<UpdateCityCommand, City>();
        }
    }
}
