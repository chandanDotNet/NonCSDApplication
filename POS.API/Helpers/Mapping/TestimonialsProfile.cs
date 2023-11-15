using AutoMapper;
using POS.Data;
using POS.Data.Dto;
using POS.MediatR.CommandAndQuery;

namespace POS.API.Helpers.Mapping
{
    public class TestimonialsProfile : Profile
    {
        public TestimonialsProfile()
        {
            CreateMap<TestimonialsDto, Testimonials>().ReverseMap();
            CreateMap<AddTestimonialsCommand, Testimonials>();
            CreateMap<UpdateTestimonialsCommand, Testimonials>();
        }
    }
}
