using AutoMapper;
using POS.Data;
using POS.Data.Dto;
using POS.MediatR.Country.Commands;

namespace POS.API.Helpers.Mapping
{
    public class CountryProfile : Profile
    {
        public CountryProfile()
        {
            CreateMap<Country, CountryDto>().ReverseMap();
            CreateMap<AddCountryCommand, Country>();
            CreateMap<UpdateCountryCommand, Country>();
        }
    }
}
