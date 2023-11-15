using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using POS.Data.Dto;
using POS.Helper;
using POS.MediatR.Customer.Commands;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace POS.MediatR.Customer.Handlers
{
    public class GetWalkInCusomerCommandHandler : IRequestHandler<GetWalkInCusomerCommand, ServiceResponse<CustomerDto>>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly PathHelper _pathHelper;
        private readonly IMapper _mapper;
        private readonly ILogger<GetWalkInCusomerCommandHandler> _logger;

        public GetWalkInCusomerCommandHandler(ICustomerRepository customerRepository,
            PathHelper pathHelper,
            IMapper mapper,
            ILogger<GetWalkInCusomerCommandHandler> logger)
        {
            _customerRepository = customerRepository;
            _pathHelper = pathHelper;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<ServiceResponse<CustomerDto>> Handle(GetWalkInCusomerCommand request, CancellationToken cancellationToken)
        {
            var customer = await _customerRepository.All.Where(c => c.IsWalkIn).FirstOrDefaultAsync();
            if (customer == null)
            {
                _logger.LogError("Customer not found.");
                return ServiceResponse<CustomerDto>.Return404();
            }
            if (!string.IsNullOrEmpty(customer.Url))
                customer.ImageUrl = string.IsNullOrWhiteSpace(customer.Url) ? "" : Path.Combine(_pathHelper.CustomerImagePath, customer.Url);
            return ServiceResponse<CustomerDto>.ReturnResultWith200(_mapper.Map<CustomerDto>(customer));
        }
    }
}
