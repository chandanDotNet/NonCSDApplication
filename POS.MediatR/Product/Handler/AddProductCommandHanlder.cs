using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using POS.Common.UnitOfWork;
using POS.Data.Dto;
using POS.Domain;
using POS.Helper;
using POS.Repository;
using System.Threading;
using System.Threading.Tasks;
using POS.MediatR.Product.Command;
using System.Collections.Generic;
using System.Linq;
using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;

namespace POS.MediatR.Product.Handler
{
    public class AddProductCommandHanlder
        : IRequestHandler<AddProductCommand, ServiceResponse<ProductDto>>
    {

        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly PathHelper _pathHelper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<UpdateProductCommandHandler> _logger;

        public AddProductCommandHanlder(IProductRepository productRepository,
            IMapper mapper,
            IUnitOfWork<POSDbContext> uow,
            PathHelper pathHelper,
            IWebHostEnvironment webHostEnvironment,
            ILogger<UpdateProductCommandHandler> logger)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _uow = uow;
            _pathHelper = pathHelper;
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
        }
        public async Task<ServiceResponse<ProductDto>> Handle(AddProductCommand request, CancellationToken cancellationToken)
        {
            var existingProduct = await _productRepository.All
                .FirstOrDefaultAsync(c => c.Name == request.Name && c.CategoryId == request.CategoryId && c.Barcode == request.Barcode);
            if (existingProduct != null)
            {
                _logger.LogError("Proudct is already exists in same category.");
                return ServiceResponse<ProductDto>.Return409("Proudct is already exists in same category.");
            }

            if (!string.IsNullOrWhiteSpace(request.Barcode))
            {
                var existProduct = await _productRepository.All
                               .FirstOrDefaultAsync(c => c.Barcode == request.Barcode);
                if (existProduct != null)
                {
                    _logger.LogError("Proudct Barcode Number is duplicate.");
                    return ServiceResponse<ProductDto>.Return409("Proudct Barcode Number is duplicate.");
                }
            }

            var product = _mapper.Map<Data.Product>(request);
            if (!string.IsNullOrWhiteSpace(request.ProductUrlData))
            {
                product.ProductUrl = $"{Guid.NewGuid()}.png";
            }

            if (!string.IsNullOrWhiteSpace(request.QRCodeUrlData))
            {
                product.QRCodeUrl = $"{Guid.NewGuid()}.png";
            }

            _productRepository.Add(product);

            if (await _uow.SaveAsync() <= 0)
            {
                _logger.LogError("Error While saving product.");
                return ServiceResponse<ProductDto>.Return500();
            }

            string contentRootPath = _webHostEnvironment.WebRootPath;
            var pathToSave = Path.Combine(contentRootPath, _pathHelper.ProductImagePath);
            var thumbnailPathToSave = Path.Combine(contentRootPath, _pathHelper.ProductThumbnailImagePath);
            if (!Directory.Exists(pathToSave))
            {
                Directory.CreateDirectory(pathToSave);
            }

            if (!Directory.Exists(thumbnailPathToSave))
            {
                Directory.CreateDirectory(thumbnailPathToSave);
            }

            if (!string.IsNullOrWhiteSpace(request.ProductUrlData))
            {
                await FileData.SaveFile(Path.Combine(pathToSave, product.ProductUrl), request.ProductUrlData);
                await FileData.SaveThumbnailFile(Path.Combine(thumbnailPathToSave, product.ProductUrl), request.ProductUrlData);
            }

            if (!string.IsNullOrWhiteSpace(request.QRCodeUrlData))
            {
                await FileData.SaveFile(Path.Combine(pathToSave, product.QRCodeUrl), request.QRCodeUrlData);
            }

            return ServiceResponse<ProductDto>.ReturnResultWith201(_mapper.Map<ProductDto>(product));
        }
    }
}
