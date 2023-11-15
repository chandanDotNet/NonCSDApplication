using POS.Data.Dto;
using POS.Helper;
using MediatR;
using System;
using System.Collections.Generic;

namespace POS.MediatR.CommandAndQuery
{
    public class AddCustomerCommand : IRequest<ServiceResponse<CustomerDto>>
    {
        public Guid? Id { get; set; }
        public string CustomerName { get; set; }
        public string ContactPerson { get; set; }
        public string Email { get; set; }
        public string Fax { get; set; }
        public string MobileNo { get; set; }
        public string PhoneNo { get; set; }
        public string Website { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public bool IsVarified { get; set; }
        public bool IsUnsubscribe { get; set; }
        public string CustomerProfile { get; set; }
        public string Address { get; set; }
        public string CountryName { get; set; }
        public string CityName { get; set; }
        public Guid? CountryId { get; set; }
        public Guid? CityId { get; set; }
        public bool IsImageUpload { get; set; }
        public string Logo { get; set; }
    }
}
