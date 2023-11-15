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
using Microsoft.EntityFrameworkCore;

namespace POS.MediatR.Handlers
{
    public class AddCustomerCommandHandler : IRequestHandler<AddCustomerCommand, ServiceResponse<CustomerDto>>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly ILogger<AddCustomerCommandHandler> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly PathHelper _pathHelper;

        public AddCustomerCommandHandler(ICustomerRepository customerRepository,
            IMapper mapper,
            IUnitOfWork<POSDbContext> uow,
            ILogger<AddCustomerCommandHandler> logger,
            IWebHostEnvironment webHostEnvironment,
            PathHelper pathHelper)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
            _uow = uow;
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
            _pathHelper = pathHelper;
        }
        public async Task<ServiceResponse<CustomerDto>> Handle(AddCustomerCommand request, CancellationToken cancellationToken)
        {
            var entity = await _customerRepository.FindBy(c => c.CustomerName == request.CustomerName).FirstOrDefaultAsync();
            if (entity != null)
            {
                _logger.LogError("Customer Name is already exist.");
                return ServiceResponse<CustomerDto>.Return422("Customer Name is already exist.");
            }
            if (request.IsImageUpload && !string.IsNullOrEmpty(request.Logo))
            {
                var imageUrl = Guid.NewGuid().ToString() + ".png";
                request.Url = imageUrl;
            }

            var customerData = _mapper.Map<POS.Data.Customer >(request);
            _customerRepository.Add(customerData);

            if (await _uow.SaveAsync() <= 0)
            {
                _logger.LogError("Error while creating Customer.");
                return ServiceResponse<CustomerDto>.Return500();
            }
            var customerDtoData = _mapper.Map<CustomerDto>(customerData);

            if (request.IsImageUpload && !string.IsNullOrWhiteSpace(request.Url))
            {
                string contentRootPath = _webHostEnvironment.WebRootPath;
                var pathToSave = Path.Combine(contentRootPath, _pathHelper.CustomerImagePath, request.Url);
                await FileData.SaveFile(pathToSave, request.Logo);
            }

            return ServiceResponse<CustomerDto>.ReturnResultWith200(customerDtoData);
        }
    }
}
