using AutoMapper;
using POS.Data.Dto;
using POS.Helper;
using POS.MediatR.CommandAndQuery;
using POS.Repository;
using MediatR;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace POS.MediatR.Handlers
{
    public class GetCustomerQueryHandler : IRequestHandler<GetCustomerQuery, ServiceResponse<CustomerDto>>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly PathHelper _pathHelper;
        private readonly IMapper _mapper;
        private readonly ILogger<GetCustomerQueryHandler> _logger;

        public GetCustomerQueryHandler(ICustomerRepository customerRepository,
            PathHelper pathHelper,
            IMapper mapper,
            ILogger<GetCustomerQueryHandler> logger)
        {
            _customerRepository = customerRepository;
            _pathHelper = pathHelper;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<ServiceResponse<CustomerDto>> Handle(GetCustomerQuery request, CancellationToken cancellationToken)
        {
            var customer = await _customerRepository.FindAsync(request.Id);
            if (customer == null)
            {
                _logger.LogError("Customer not found.", request.Id);
                return ServiceResponse<CustomerDto>.Return404();
            }
            if (!string.IsNullOrEmpty(customer.Url))
                customer.ImageUrl = string.IsNullOrWhiteSpace(customer.Url) ? "" : Path.Combine(_pathHelper.CustomerImagePath, customer.Url);
            return ServiceResponse<CustomerDto>.ReturnResultWith200(_mapper.Map<CustomerDto>(customer));
        }
    }
}
