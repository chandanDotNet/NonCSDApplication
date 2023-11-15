using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using POS.Data.Dto;
using POS.Helper;
using POS.MediatR.Product.Command;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace POS.MediatR.Product.Handler
{
    public class GetProductCommandHandler : IRequestHandler<GetProductCommand, ServiceResponse<ProductDto>>
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<GetProductCommandHandler> _logger;
        private readonly IMapper _mapper;
        private readonly PathHelper _pathHelper;

        public GetProductCommandHandler(IProductRepository productRepository,
            ILogger<GetProductCommandHandler> logger,
            IMapper mapper,
            PathHelper pathHelper)
        {
            _productRepository = productRepository;
            _logger = logger;
            _mapper = mapper;
            _pathHelper = pathHelper;
        }

        public async Task<ServiceResponse<ProductDto>> Handle(GetProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.AllIncluding(c => c.ProductTaxes).FirstOrDefaultAsync(c => c.Id == request.Id);
            if (product == null)
            {
                _logger.LogError("Not found");
                return ServiceResponse<ProductDto>.Return404();

            }

            if (!string.IsNullOrWhiteSpace(product.ProductUrl))
            {
                product.ProductUrl = Path.Combine(_pathHelper.ProductImagePath, product.ProductUrl);
            }
            if (!string.IsNullOrWhiteSpace(product.QRCodeUrl))
            {
                product.QRCodeUrl = Path.Combine(_pathHelper.ProductImagePath, product.QRCodeUrl);
            }
            return ServiceResponse<ProductDto>.ReturnResultWith200(_mapper.Map<ProductDto>(product));
        }
    }
}
