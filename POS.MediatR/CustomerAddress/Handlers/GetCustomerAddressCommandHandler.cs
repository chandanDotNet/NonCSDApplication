using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using POS.Data.Dto;
using POS.Helper;
using POS.MediatR.CustomerAddress.Commands;
using POS.MediatR.CustomerAddress.Handlers;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace POS.MediatR.CustomerAddress.Handlers
{
    public class GetCustomerAddressCommandHandler : IRequestHandler<GetCustomerAddressCommand, ServiceResponse<CustomerAddressDto>>
    {
        private readonly ICustomerAddressRepository _customerAddressRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetCustomerAddressCommandHandler> _logger;
        public GetCustomerAddressCommandHandler(
            ICustomerAddressRepository customerAddressRepository,
            IMapper mapper,
            ILogger<GetCustomerAddressCommandHandler> logger
            )
        {
            _customerAddressRepository = customerAddressRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ServiceResponse<CustomerAddressDto>> Handle(GetCustomerAddressCommand request, CancellationToken cancellationToken)
        {
            var entityDto = await _customerAddressRepository.FindBy(c => c.CustomerId == request.CustomerId)
                .ProjectTo<CustomerAddressDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
            if (entityDto == null)
            {
                _logger.LogError("Counter is not exists");
                return ServiceResponse<CustomerAddressDto>.Return404();
            }
            return ServiceResponse<CustomerAddressDto>.ReturnResultWith200(entityDto);
        }
    }
}