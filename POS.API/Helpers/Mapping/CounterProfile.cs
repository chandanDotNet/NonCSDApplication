using AutoMapper;
using POS.Data.Dto;
using POS.Data;
using POS.MediatR.Counter.Commands;


namespace POS.API.Helpers.Mapping
{
    public class CounterProfile : Profile
    {
        public CounterProfile()
        {
            CreateMap<Counter, CounterDto>().ReverseMap();
            CreateMap<AddCounterCommand, Counter>();
            //CreateMap<UpdateCityCommand, City>();
        }
    }
}
