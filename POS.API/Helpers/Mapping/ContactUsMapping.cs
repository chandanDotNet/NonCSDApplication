using AutoMapper;
using POS.Data;
using POS.Data.Dto;
using POS.MediatR.CommandAndQuery;

namespace POS.API.Helpers
{
    public class ContactUsMapping: Profile
    {
        public ContactUsMapping()
        {
            CreateMap<ContactRequest, ContactUsDto>().ReverseMap();
            CreateMap<AddContactUsCommand, ContactRequest>();
        }
    }
}
