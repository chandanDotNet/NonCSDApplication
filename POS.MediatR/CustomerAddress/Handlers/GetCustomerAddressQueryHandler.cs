using MediatR;
using POS.MediatR.CommandAndQuery;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace POS.MediatR.CustomerAddress.Handlers
{
    internal class GetCustomerAddressQueryHandler : IRequestHandler<GetCustomerAddressQuery, CustomerAddressList>
    {
        private readonly ICustomerAddressRepository _cutomerAddressRepository;
        public GetCustomerAddressQueryHandler(ICustomerAddressRepository customerAddressRepository)
        {
            _cutomerAddressRepository = customerAddressRepository;
        }
        public async Task<CustomerAddressList> Handle(GetCustomerAddressQuery request, CancellationToken cancellationToken)
        {
            return await _cutomerAddressRepository.GetCustomerAddresses(request.CustomerAddressResource);
        }
    }
}
