using AutoMapper;
using POS.Common.UnitOfWork;
using POS.Data;
using POS.Data.Dto;
using POS.Domain;
using POS.Helper;
using POS.MediatR.CommandAndQuery;
using POS.Repository;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace POS.MediatR.Handlers
{
    public class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand, ServiceResponse<CustomerDto>>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly ILogger<UpdateCustomerCommandHandler> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly PathHelper _pathHelper;

        public UpdateCustomerCommandHandler(
            ICustomerRepository customerRepository,
            IMapper mapper,
            IUnitOfWork<POSDbContext> uow,
            ILogger<UpdateCustomerCommandHandler> logger,
            IWebHostEnvironment webHostEnvironment,
            PathHelper pathHelper
            )
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
            _uow = uow;
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
            _pathHelper = pathHelper;
        }
        public async Task<ServiceResponse<CustomerDto>> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer = await _customerRepository.FindAsync(request.Id.Value);
            if (customer.IsWalkIn)
            {
                _logger.LogError("Customer can not be Edited because it is Walk In Customer");
                return ServiceResponse<CustomerDto>.Return409("Customer can not be Edited because it is Walk In Customer");
            }

            if (customer == null)
            {
                _logger.LogError("Customer does not exists.");
                return ServiceResponse<CustomerDto>.Return404();
            }

            if (request.IsImageUpload)
            {
                if (!string.IsNullOrEmpty(request.Logo))
                {
                    request.Url = Guid.NewGuid().ToString() + ".png";
                }
                else
                {
                    request.Url = "";
                }
            }
            else
            {
                request.Url = customer.Url;
            }

            var oldImageUrl = customer.Url;

            var customerToAdd = _mapper.Map(request, customer);


            _customerRepository.Update(customerToAdd);
            if (await _uow.SaveAsync() <= 0)
            {
                _logger.LogError("Error while updated Customer.");
                return ServiceResponse<CustomerDto>.Return500();
            }

            if (request.IsImageUpload)
            {
                string contentRootPath = _webHostEnvironment.WebRootPath;
                // delete old file
                if (!string.IsNullOrWhiteSpace(oldImageUrl)
                    && File.Exists(Path.Combine(contentRootPath, _pathHelper.CustomerImagePath, oldImageUrl)))
                {
                    FileData.DeleteFile(Path.Combine(contentRootPath, _pathHelper.CustomerImagePath, oldImageUrl));
                }
                // save new file
                if (!string.IsNullOrWhiteSpace(request.Logo))
                {
                    var pathToSave = Path.Combine(contentRootPath, _pathHelper.CustomerImagePath, customer.Url);

                    await FileData.SaveFile(pathToSave, request.Logo);
                }
            }

            var customerToReturn = _mapper.Map<CustomerDto>(customerToAdd);
            return ServiceResponse<CustomerDto>.ReturnResultWith201(customerToReturn);
        }
    }
}
