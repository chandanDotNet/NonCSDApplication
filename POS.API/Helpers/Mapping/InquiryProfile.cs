using AutoMapper;
using POS.Data;
using POS.Data.Dto;
using POS.MediatR.CommandAndQuery;

namespace POS.API.Helpers
{
    public class InquiryProfile: Profile
    {
        public InquiryProfile()
        {
            CreateMap<InquiryProduct, InquiryProductDto>().ReverseMap();
            CreateMap<Inquiry, InquiryDto>().ReverseMap();
            CreateMap<AddInquiryCommand, Inquiry>().ReverseMap();
            CreateMap<UpdateInquiryCommand, Inquiry>().ReverseMap();
        }
    }
}
