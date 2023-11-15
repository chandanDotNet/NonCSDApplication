using MediatR;
using POS.MediatR.Customer.Commands;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace POS.MediatR.Customer.Handlers
{

    public class GetCustomerPaymentsQueryHandler
     : IRequestHandler<GetCustomerPaymentsQuery, CustomerPaymentList>
    {
        private readonly ICustomerRepository _customerRepository;

        public GetCustomerPaymentsQueryHandler(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }
        public async Task<CustomerPaymentList> Handle(GetCustomerPaymentsQuery request, CancellationToken cancellationToken)
        {
            return await _customerRepository.GetCustomersPayment(request.CustomerResource);
        }
    }
}
