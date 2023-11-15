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
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace POS.MediatR.Handlers
{
    public class AddSupplierCommandHandler : IRequestHandler<AddSupplierCommand, ServiceResponse<SupplierDto>>
    {
        private readonly ISupplierRepository _supplierRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly ILogger<AddSupplierCommandHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly PathHelper _pathHelper;

        public AddSupplierCommandHandler(ISupplierRepository supplierRepository,
            ILogger<AddSupplierCommandHandler> logger,
            IUnitOfWork<POSDbContext> uow,
            IMapper mapper,
              IWebHostEnvironment webHostEnvironment,
              PathHelper pathHelper)
        {
            _supplierRepository = supplierRepository;
            _uow = uow;
            _logger = logger;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
            _pathHelper = pathHelper;
        }

        public async Task<ServiceResponse<SupplierDto>> Handle(AddSupplierCommand request, CancellationToken cancellationToken)
        {
            if (request.IsImageUpload && !string.IsNullOrEmpty(request.Logo))
            {
                var imageUrl = Guid.NewGuid().ToString() + ".png";
                request.Url = imageUrl;
            }

            var entity = await _supplierRepository.FindBy(c => c.SupplierName == request.SupplierName).FirstOrDefaultAsync();
            if (entity != null)
            {
                _logger.LogError("Supplier Name is already exist.");
                return ServiceResponse<SupplierDto>.Return422("Supplier Name is already exist.");
            }
            entity = _mapper.Map<Data.Supplier>(request);
            _supplierRepository.Add(entity);

            if (await _uow.SaveAsync() <= 0)
            {
                _logger.LogError("Error to Save Supplier");
                return ServiceResponse<SupplierDto>.Return500();
            }

            if (request.IsImageUpload && !string.IsNullOrWhiteSpace(entity.Url))
            {
                string contentRootPath = _webHostEnvironment.WebRootPath;
                var pathToSave = Path.Combine(contentRootPath, _pathHelper.SupplierImagePath, entity.Url);
                await FileData.SaveFile(pathToSave, request.Logo);
            }
            return ServiceResponse<SupplierDto>.ReturnResultWith200(_mapper.Map<SupplierDto>(entity));
        }
    }
}
