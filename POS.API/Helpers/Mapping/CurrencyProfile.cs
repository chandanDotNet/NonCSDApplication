using AutoMapper;
using POS.Data;
using POS.Data.Dto;
using POS.MediatR.Currency.Commands;

namespace POS.API.Helpers.Mapping
{
    public class CurrencyProfile : Profile
    {
        public CurrencyProfile()
        {
            CreateMap<CurrencyDto, Currency>().ReverseMap();
            CreateMap<AddCurrencyCommand, Currency>();
            CreateMap<UpdateCurrencyCommand, Currency>();
        }
    }
}
