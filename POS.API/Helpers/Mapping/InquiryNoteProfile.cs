using AutoMapper;
using POS.Data.Dto;
using POS.Data.Entities;
using POS.MediatR.CommandAndQuery;

namespace POS.API.Helpers.Mapping
{
    public class InquiryNoteProfile : Profile
    {
        public InquiryNoteProfile()
        {
            CreateMap<AddInquiryNoteCommand, InquiryNote>();
            CreateMap<InquiryNoteDto, InquiryNote>().ReverseMap();
            CreateMap<UpdateInquiryNoteCommand, InquiryNote>();
        }
    }
}
