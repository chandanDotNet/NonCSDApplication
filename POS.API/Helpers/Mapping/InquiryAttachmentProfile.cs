using AutoMapper;
using POS.Data.Dto;
using POS.Data.Entities;

namespace POS.API.Helpers.Mapping
{
    public class InquiryAttachmentProfile : Profile
    {
        public InquiryAttachmentProfile()
        {
            CreateMap<InquiryAttachmentDto, InquiryAttachment>().ReverseMap();
        }
    }
}
