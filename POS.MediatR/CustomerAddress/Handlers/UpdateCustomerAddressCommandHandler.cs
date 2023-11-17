using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using POS.Common.UnitOfWork;
using POS.Data.Dto;
using POS.Domain;
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
    public class UpdateCustomerAddressCommandHandler : IRequestHandler<UpdateCustomerAddressCommand, ServiceResponse<CustomerAddressDto>>
    {
        private readonly ICustomerAddressRepository _customerAddressRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly ILogger<UpdateCustomerAddressCommandHandler> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IMapper _mapper;

        public UpdateCustomerAddressCommandHandler(
           ICustomerAddressRepository customerAddressRepository,
           IUnitOfWork<POSDbContext> uow,
           ILogger<UpdateCustomerAddressCommandHandler> logger,
           IWebHostEnvironment webHostEnvironment,
        IMapper mapper
           )
        {
            _customerAddressRepository = customerAddressRepository;
            _uow = uow;
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<CustomerAddressDto>> Handle(UpdateCustomerAddressCommand request, CancellationToken cancellationToken)
        {
            var entityExist = await _customerAddressRepository.FindBy(c => c.HouseNo == request.HouseNo && c.Id != request.Id)
             .FirstOrDefaultAsync();
            if (entityExist != null)
            {
                _logger.LogError("Customer Address Already Exist.");
                return ServiceResponse<CustomerAddressDto>.Return409("Customer Address Already Exist.");
            }
            entityExist = await _customerAddressRepository.FindBy(v => v.Id == request.Id).FirstOrDefaultAsync();
            entityExist.HouseNo = request.HouseNo;
            entityExist.StreetDetails = request.StreetDetails;
            entityExist.LandMark = request.LandMark;
            entityExist.Type = request.Type;
            entityExist.IsPrimary = request.IsPrimary;

            _customerAddressRepository.Update(entityExist);

            //remove other as Primary Address
            if (entityExist.IsPrimary)
            {
                var defaultPrimaryAddressSettings = await _customerAddressRepository.All.Where(c => c.CustomerId == request.CustomerId && c.IsPrimary).ToListAsync();
                defaultPrimaryAddressSettings.ForEach(c => c.IsPrimary = false);
                _customerAddressRepository.UpdateRange(defaultPrimaryAddressSettings);
            }

            if (await _uow.SaveAsync() <= 0)
            {
                return ServiceResponse<CustomerAddressDto>.Return500();
            }

            var result = _mapper.Map<CustomerAddressDto>(entityExist);

            return ServiceResponse<CustomerAddressDto>.ReturnResultWith200(result);
        }
    }
}