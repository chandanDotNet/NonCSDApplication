using POS.Data.Dto;
using POS.Helper;
using MediatR;
using System;
using System.Collections.Generic;

namespace POS.MediatR.CommandAndQuery
{
    public class AddInquiryCommand: IRequest<ServiceResponse<InquiryDto>>
    {
        public string CompanyName { get; set; }
        public string ContactPerson { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string MobileNo { get; set; }
        public string Address { get; set; }
        public string Message { get; set; }
        public string CityName { get; set; }
        public string Website { get; set; }
        public string CountryName { get; set; }
        public Guid? CityId { get; set; }
        public Guid? CountryId { get; set; }
        public Guid InquirySourceId { get; set; }
        public Guid InquiryStatusId { get; set; }
        public List<InquiryProductDto> InquiryProducts { get; set; } = new List<InquiryProductDto>();
        public Guid? AssignTo { get; set; }
    }
}
