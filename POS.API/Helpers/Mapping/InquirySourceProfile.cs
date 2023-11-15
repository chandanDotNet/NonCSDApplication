using AutoMapper;
using POS.Data;
using POS.Data.Dto;
using POS.MediatR.CommandAndQuery;

namespace POS.API.Helpers
{
    public class InquirySourceProfile : Profile
    {
        public InquirySourceProfile()
        {
            CreateMap<InquirySource, InquirySourceDto>().ReverseMap();
            CreateMap<AddInquirySourceCommand, InquirySource>();
            CreateMap<UpdateInquirySourceCommand, InquirySource>();
        }
    }
}
