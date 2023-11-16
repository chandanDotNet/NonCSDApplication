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
    public class UpdateCustomerAddressCommand : IRequest<ServiceResponse<CustomerAddressDto>>
    {
        public Guid Id { get; set; }
        public string HouseNo { get; set; }
        public string StreetDetails { get; set; }
        public string LandMark { get; set; }
        public string Type { get; set; }
        public bool IsPrimary { get; set; }
    }
}
