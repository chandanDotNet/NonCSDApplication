using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using POS.Common.UnitOfWork;
using POS.Data.Dto;
using POS.Domain;
using POS.Helper;
using POS.MediatR.Product.Command;
using POS.Repository;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace POS.MediatR.Customer.Handlers
{
    public class GetProductsDetailCommandHandler : IRequestHandler<GetProductsDetailCommand, ServiceResponse<List<ProductDto>>>
    {

        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly PathHelper _pathHelper;

        public GetProductsDetailCommandHandler(
            IProductRepository productRepository,
            IMapper mapper,
            IUnitOfWork<POSDbContext> uow,
            PathHelper pathHelper
            )
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _uow = uow;
            _pathHelper = pathHelper;
        }

        public async Task<ServiceResponse<List<ProductDto>>> Handle(GetProductsDetailCommand request, CancellationToken cancellationToken)
        {
            var entities = await _productRepository.All
              .ProjectTo<ProductDto>(_mapper.ConfigurationProvider)
              .ToListAsync();

            return ServiceResponse<List<ProductDto>>.ReturnResultWith200(_mapper.Map<List<ProductDto>>(entities));
        }
    }
}
