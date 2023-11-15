using MediatR;
using POS.MediatR.Commands;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace POS.MediatR.Handlers
{
    public class GetCustomerPaymentsReportQueryHandler : IRequestHandler<GetCustomerPaymentsReportQuery, CustomerPaymentList>
    {
        private readonly ICustomerRepository _customerRepository;

        public GetCustomerPaymentsReportQueryHandler(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }
        public async Task<CustomerPaymentList> Handle(GetCustomerPaymentsReportQuery request, CancellationToken cancellationToken)
        {
            return await _customerRepository.GetCustomersPaymentReport(request.CustomerResource);
        }
    }
}
