using AutoMapper;
using POS.Data;
using POS.Data.Dto;
using POS.Data.Entities;
using POS.MediatR.CommandAndQuery;

namespace POS.API.Helpers.Mapping
{
    public class InquiryStatusProfile : Profile
    {
        public InquiryStatusProfile()
        {
            CreateMap<InquiryStatus, InquiryStatusDto>().ReverseMap();
            CreateMap<AddInquiryStatusCommand, InquiryStatus>();
        }
    }
}
