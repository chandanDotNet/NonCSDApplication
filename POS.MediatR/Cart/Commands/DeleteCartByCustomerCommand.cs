using MediatR;
using POS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.MediatR.Cart.Commands
{
    public class DeleteCartByCustomerCommand : IRequest<ServiceResponse<bool>>
    {
        public Guid CustomerId { get; set; }

    }
}
