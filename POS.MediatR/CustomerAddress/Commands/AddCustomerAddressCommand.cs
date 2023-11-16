using MediatR;
using POS.Data.Dto;
using POS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.MediatR.CustomerAddress.Commands
{
    public class AddCustomerAddressCommand : IRequest<ServiceResponse<CustomerAddressDto>>
    {
        public string HouseNo { get; set; }
        public string StreetDetails { get; set; }
        public string LandMark { get; set; }
        public string Type { get; set; }
        public Guid? CustomerId { get; set; }
        public bool IsPrimary { get; set; }
    }
}
