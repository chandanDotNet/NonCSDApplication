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
using POS.Data;

namespace POS.MediatR.Product.Handler
{
    public class UpdateProductCommandHandler
      : IRequestHandler<UpdateProductCommand, ServiceResponse<ProductDto>>
    {

        private readonly IProductRepository _productRepository;
        private readonly IProductTaxRepository _productTaxRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly PathHelper _pathHelper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<UpdateProductCommandHandler> _logger;

        public UpdateProductCommandHandler(IProductRepository productRepository,
            IProductTaxRepository productTaxRepository,
            IMapper mapper,
            IUnitOfWork<POSDbContext> uow,
            PathHelper pathHelper,
            IWebHostEnvironment webHostEnvironment,
            ILogger<UpdateProductCommandHandler> logger)
        {
            _productRepository = productRepository;
            _productTaxRepository = productTaxRepository;
            _mapper = mapper;
            _uow = uow;
            _pathHelper = pathHelper;
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
        }
        public async Task<ServiceResponse<ProductDto>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var existingProduct = await _productRepository.All
                .FirstOrDefaultAsync(c => c.Name == request.Name && c.CategoryId == request.CategoryId && c.Id != request.Id);
            if (existingProduct != null)
            {
                _logger.LogError("Proudct is already exists in same category.");
                return ServiceResponse<ProductDto>.Return409("Proudct is already exists in same category.");
            }

            if (!string.IsNullOrWhiteSpace(request.Barcode))
            {
                var existProduct = await _productRepository.All
                               .FirstOrDefaultAsync(c => c.Barcode == request.Barcode && c.Id != request.Id);
                if (existProduct != null)
                {
                    _logger.LogError("Proudct Barcode Number is duplicate.");
                    return ServiceResponse<ProductDto>.Return409("Proudct Barcode Number is duplicate.");
                }
            }

            existingProduct = await _productRepository.All.FirstOrDefaultAsync(c => c.Id == request.Id);

            if (existingProduct == null)
            {
                _logger.LogError("Proudct does not exists.");
                return ServiceResponse<ProductDto>.Return404("Proudct does not exists.");
            }

            var oldProductUrl = existingProduct.ProductUrl;
            var oldQRCodeUrl = existingProduct.QRCodeUrl;

            if (request.IsProductImageUpload)
            {
                if (!string.IsNullOrWhiteSpace(request.ProductUrlData))
                {
                    existingProduct.ProductUrl = $"{Guid.NewGuid()}.png";
                }
                else
                {
                    existingProduct.ProductUrl = null;
                }
            }

            if (request.IsQrCodeUpload)
            {
                if (!string.IsNullOrWhiteSpace(request.QRCodeUrlData))
                {
                    existingProduct.QRCodeUrl = $"{Guid.NewGuid()}.png";
                }
                else
                {
                    existingProduct.QRCodeUrl = null;
                }
            }

            var productTaxes = _productTaxRepository.All.Where(c => c.ProductId == request.Id).ToList();
            var productTaxToAdd = request.ProductTaxes.Where(c => !productTaxes.Select(c => c.TaxId).Contains(c.TaxId)).ToList();
            _productTaxRepository.AddRange(_mapper.Map<List<ProductTax>>(productTaxToAdd));
            var productTaxToDelete = productTaxes.Where(c => !request.ProductTaxes.Select(cs => cs.TaxId).Contains(c.TaxId)).ToList();
            _productTaxRepository.RemoveRange(productTaxToDelete);
            request.ProductTaxes = null;

            _mapper.Map(request, existingProduct);
            _productRepository.Update(existingProduct);

            if (await _uow.SaveAsync() <= 0)
            {
                _logger.LogError("Error While saving Proudct.");
                return ServiceResponse<ProductDto>.Return500();
            }

            try
            {
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

                if (request.IsProductImageUpload)
                {
                    if (!string.IsNullOrWhiteSpace(request.ProductUrlData))
                    {
                        await FileData.SaveFile(Path.Combine(pathToSave, existingProduct.ProductUrl), request.ProductUrlData);
                        await FileData.SaveThumbnailFile(Path.Combine(thumbnailPathToSave, existingProduct.ProductUrl), request.ProductUrlData);
                    }

                    if (!string.IsNullOrWhiteSpace(oldProductUrl))
                    {
                        FileData.DeleteFile(Path.Combine(pathToSave, oldProductUrl));
                        FileData.DeleteFile(Path.Combine(thumbnailPathToSave, oldProductUrl));
                    }
                }

                if (request.IsQrCodeUpload)
                {
                    if (!string.IsNullOrWhiteSpace(request.QRCodeUrlData))
                    {
                        await FileData.SaveFile(Path.Combine(pathToSave, existingProduct.QRCodeUrl), request.QRCodeUrlData);
                    }
                    if (!string.IsNullOrWhiteSpace(oldQRCodeUrl))
                    {
                        FileData.DeleteFile(Path.Combine(pathToSave, oldQRCodeUrl));
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error While saving Proudct Image.", ex);
            }

            var entityDto = _mapper.Map<ProductDto>(existingProduct);
            return ServiceResponse<ProductDto>.ReturnResultWith201(entityDto);
        }
    }
}
