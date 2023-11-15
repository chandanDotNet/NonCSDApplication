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
    public class GetCustomerAddressCommand : IRequest<ServiceResponse<CustomerAddressDto>>
    {
        public Guid CustomerId { get; set; }
    }
}