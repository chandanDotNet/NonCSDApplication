using MediatR;
using POS.Data;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.MediatR.Customer.Commands
{
    public class GetCustomerPaymentsQuery : IRequest<CustomerPaymentList>
    {
        public CustomerResource CustomerResource { get; set; }
    }
}
