using AutoMapper;
using POS.Common.UnitOfWork;
using POS.Data;
using POS.Data.Dto;
using POS.Domain;
using POS.Helper;
using POS.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using POS.MediatR.Category.Commands;

namespace POS.MediatR.Category.Handlers
{
    public class UpdateProductCategoryCommandHandler
     : IRequestHandler<UpdateProductCategoryCommand, ServiceResponse<ProductCategoryDto>>
    {
        private readonly IProductCategoryRepository _productCategoryRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateProductCategoryCommandHandler> _logger;
        public UpdateProductCategoryCommandHandler(
           IProductCategoryRepository productCategoryRepository,
            IMapper mapper,
            IUnitOfWork<POSDbContext> uow,
            ILogger<UpdateProductCategoryCommandHandler> logger
            )
        {
            _productCategoryRepository = productCategoryRepository;
            _mapper = mapper;
            _uow = uow;
            _logger = logger;
        }
        public async Task<ServiceResponse<ProductCategoryDto>> Handle(UpdateProductCategoryCommand request, CancellationToken cancellationToken)
        {
            var existingEntity = await _productCategoryRepository
                .All
                .FirstOrDefaultAsync(c => c.Name == request.Name
                && c.ParentId == request.ParentId
                && c.Id != request.Id);
            if (existingEntity != null)
            {
                _logger.LogError("Product Category Already Exist");
                return ServiceResponse<ProductCategoryDto>.Return409("Product Category Already Exist.");
            }

            existingEntity = await _productCategoryRepository.FindAsync(request.Id);
            var entity = _mapper.Map(request, existingEntity);
            _productCategoryRepository.Update(entity);
            if (await _uow.SaveAsync() <= 0)
            {

                _logger.LogError("Error While saving Product Category.");
                return ServiceResponse<ProductCategoryDto>.Return500();
            }
            return ServiceResponse<ProductCategoryDto>.ReturnResultWith200(_mapper.Map<ProductCategoryDto>(entity));
        }
    }
}
