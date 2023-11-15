using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using POS.Common.UnitOfWork;
using POS.Data.Dto;
using POS.Domain;
using POS.Helper;
using POS.MediatR.Brand.Command;
using POS.MediatR.Tax.Commands;
using POS.Repository;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace POS.MediatR.Brand.Handler
{
    public class AddBrandCommandHandler : IRequestHandler<AddBrandCommand, ServiceResponse<BrandDto>>
    {
        private readonly IBrandRepository _brandRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<AddBrandCommandHandler> _logger;
        private readonly PathHelper _pathHelper;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AddBrandCommandHandler(
           IBrandRepository brandRepository,
            IMapper mapper,
            IUnitOfWork<POSDbContext> uow,
            ILogger<AddBrandCommandHandler> logger,
            PathHelper pathHelper,
            IWebHostEnvironment webHostEnvironment
            )
        {
            _brandRepository = brandRepository;
            _mapper = mapper;
            _uow = uow;
            _logger = logger;
            _pathHelper = pathHelper;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<ServiceResponse<BrandDto>> Handle(AddBrandCommand request, CancellationToken cancellationToken)
        {

            var existingEntity = await _brandRepository.FindBy(c => c.Name == request.Name).FirstOrDefaultAsync();
            if (existingEntity != null)
            {
                _logger.LogError("Brand Already Exist");
                return ServiceResponse<BrandDto>.Return409("Brand Already Exist.");
            }
            var entity = _mapper.Map<Data.Brand>(request);

            if (!string.IsNullOrWhiteSpace(request.ImageUrlData))
            {
                entity.ImageUrl = Guid.NewGuid().ToString() + ".png";
            }

            _brandRepository.Add(entity);
            if (await _uow.SaveAsync() <= 0)
            {

                _logger.LogError("Save Page have Error");
                return ServiceResponse<BrandDto>.Return500();
            }

            if (!string.IsNullOrWhiteSpace(request.ImageUrlData))
            {
                var pathToSave = Path.Combine(_webHostEnvironment.WebRootPath, _pathHelper.BrandImagePath);
                if (!Directory.Exists(pathToSave))
                {
                    Directory.CreateDirectory(pathToSave);
                }
                await FileData.SaveFile(Path.Combine(pathToSave, entity.ImageUrl), request.ImageUrlData);
            }
            var entityToReturn = _mapper.Map<BrandDto>(entity);
            if (!string.IsNullOrWhiteSpace(request.ImageUrlData))
            {
                entityToReturn.ImageUrl = Path.Combine(_pathHelper.BrandImagePath, entityToReturn.ImageUrl);
            }
            return ServiceResponse<BrandDto>.ReturnResultWith200(entityToReturn);
        }
    }
}
