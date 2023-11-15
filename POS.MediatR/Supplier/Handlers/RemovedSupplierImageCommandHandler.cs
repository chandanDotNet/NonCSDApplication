using AutoMapper;
using POS.Common.UnitOfWork;
using POS.Data.Dto;
using POS.Domain;
using POS.Helper;
using POS.MediatR.CommandAndQuery;
using POS.Repository;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace POS.MediatR.Handlers
{
    public class RemovedSupplierImageCommandHandler : IRequestHandler<RemovedSupplierImageCommand, ServiceResponse<SupplierDto>>
    {
        private readonly ISupplierRepository _supplierRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly ILogger<RemovedSupplierImageCommandHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly PathHelper _pathHelper;
        public RemovedSupplierImageCommandHandler(ISupplierRepository supplierRepository,
            ILogger<RemovedSupplierImageCommandHandler> logger,
            IUnitOfWork<POSDbContext> uow,
            IMapper mapper,
            IWebHostEnvironment webHostEnvironment,
            PathHelper pathHelper
            )
        {
            _supplierRepository = supplierRepository;
            _uow = uow;
            _logger = logger;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
            _pathHelper = pathHelper;
        }

        public async Task<ServiceResponse<SupplierDto>> Handle(RemovedSupplierImageCommand request, CancellationToken cancellationToken)
        {
            var entity = _supplierRepository.Find(request.Id);
            if (entity == null)
            {
                _logger.LogError("Supplier does not exist.");
                return ServiceResponse<SupplierDto>.Return404("Supplier does not found.");
            }
            var oldImageUrl = entity.Url;
            entity.Url = string.Empty;
            _supplierRepository.Update(entity);
            if (await _uow.SaveAsync() <= 0)
            {
                _logger.LogError("Error to Save Supplier");
                return ServiceResponse<SupplierDto>.Return500();
            }

            // delete supplier image
            string contentRootPath = _webHostEnvironment.WebRootPath;
            var imgPath = Path.Combine(contentRootPath, _pathHelper.SupplierImagePath, oldImageUrl);
            if (File.Exists(imgPath))
            {
                FileData.DeleteFile(imgPath);
            }
            return ServiceResponse<SupplierDto>.ReturnResultWith200(_mapper.Map<SupplierDto>(entity));

        }


    }
}
