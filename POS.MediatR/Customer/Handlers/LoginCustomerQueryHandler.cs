using MediatR;
using POS.MediatR.CommandAndQuery;
using POS.MediatR.Commands;
using POS.MediatR.Customer.Commands;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace POS.MediatR.Handlers
{
    public class LoginCustomerQueryHandler : IRequestHandler<LoginCustomerQuery, CustomerList>
    {

        private readonly ICustomerRepository _customerRepository;

        public LoginCustomerQueryHandler(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }
        public async Task<CustomerList> Handle(LoginCustomerQuery request, CancellationToken cancellationToken)
        {
            return await _customerRepository.GetCustomersData(request.CustomerResource);
        }

    }
}
