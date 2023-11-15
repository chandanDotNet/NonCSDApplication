using AutoMapper;
using MediatR;
using POS.Data.Dto;
using POS.Helper;
using POS.MediatR.Category.Commands;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace POS.MediatR.Category.Handlers
{
    public class GetProductCategoryByIdQueryHandler
        : IRequestHandler<GetProductCategoryByIdQuery, ServiceResponse<ProductCategoryDto>>
    {
        private readonly IProductCategoryRepository _productCategoryRepository;
        private readonly IMapper _mapper;

        public GetProductCategoryByIdQueryHandler(
            IProductCategoryRepository productCategoryRepository,
            IMapper mapper)
        {
            _productCategoryRepository = productCategoryRepository;
            _mapper = mapper;
        }
        public async Task<ServiceResponse<ProductCategoryDto>> Handle(GetProductCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            var category = await _productCategoryRepository.FindAsync(request.Id);
            if (category == null)
            {
                return ServiceResponse<ProductCategoryDto>.Return404();
            }

            return ServiceResponse<ProductCategoryDto>.ReturnResultWith200(_mapper.Map<ProductCategoryDto>(category));
        }
    }
}
