using MediatR;
using POS.Data;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.MediatR.Commands
{
    public class GetCustomerPaymentsReportQuery : IRequest<CustomerPaymentList>
    {
        public CustomerResource CustomerResource { get; set; }
    }
}
