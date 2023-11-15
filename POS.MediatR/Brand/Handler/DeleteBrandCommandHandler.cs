using POS.Common.UnitOfWork;
using POS.Data.Dto;
using POS.Domain;
using POS.Helper;
using POS.MediatR.CommandAndQuery;
using POS.Repository;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using POS.MediatR.Tax.Commands;
using POS.MediatR.Brand.Command;

namespace POS.MediatR.Brand.Handler
{
    public class DeleteBrandCommandHandler
        : IRequestHandler<DeleteBrandCommand, ServiceResponse<bool>>
    {
        private readonly IBrandRepository _brandRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly ILogger<DeleteBrandCommandHandler> _logger;

        public DeleteBrandCommandHandler(
           IBrandRepository brandRepository,
           IProductRepository productRepository,
            IUnitOfWork<POSDbContext> uow,
            ILogger<DeleteBrandCommandHandler> logger
            )
        {
            _brandRepository = brandRepository;
            _productRepository = productRepository;
            _uow = uow;
            _logger = logger;
        }

        public async Task<ServiceResponse<bool>> Handle(DeleteBrandCommand request, CancellationToken cancellationToken)
        {
            var entityExist = await _brandRepository.FindAsync(request.Id);
            if (entityExist == null)
            {
                _logger.LogError("Brand Does not exists");
                return ServiceResponse<bool>.Return404("Brand  Does not exists");
            }

            var exitingProduct = _productRepository.AllIncluding(c => c.Brand).Any(c => c.Brand.Id == entityExist.Id);
            if (exitingProduct)
            {
                _logger.LogError("Brand can not be Deleted because it is use in product");
                return ServiceResponse<bool>.Return409("Brand can not be Deleted because it is use in product.");
            }

            _brandRepository.Delete(entityExist);
            if (await _uow.SaveAsync() <= 0)
            {
                _logger.LogError("Error While saving Brand.");
                return ServiceResponse<bool>.Return500();
            }

            return ServiceResponse<bool>.ReturnSuccess();
        }
    }
}
