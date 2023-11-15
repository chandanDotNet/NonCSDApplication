using AutoMapper;
using POS.Data.Dto;
using POS.Data.Entities;
using POS.MediatR.CommandAndQuery;

namespace POS.API.Helpers.Mapping
{
    public class InquiryActivityProfile : Profile
    {
        public InquiryActivityProfile()
        {
            CreateMap<InquiryActivity, InquiryActivityDto>().ReverseMap();
            CreateMap<AddInquiryActivityCommand, InquiryActivity>();
            CreateMap<UpdateInquiryActivityCommand, InquiryActivity>();
        }
    }
}
