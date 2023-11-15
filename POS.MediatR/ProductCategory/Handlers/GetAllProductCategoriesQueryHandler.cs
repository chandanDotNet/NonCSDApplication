using AutoMapper;
using POS.Data.Dto;
using POS.MediatR.CommandAndQuery;
using POS.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;

namespace POS.MediatR.Handler
{
    public class GetAllProductCategoriesQueryHandler : IRequestHandler<GetAllProductCategoriesQuery, List<ProductCategoryDto>>
    {
        private readonly IProductCategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public GetAllProductCategoriesQueryHandler(IProductCategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }
        public async Task<List<ProductCategoryDto>> Handle(GetAllProductCategoriesQuery request, CancellationToken cancellationToken)
        {
            var categories = await _categoryRepository.All
                .Where(c => request.IsDropDown || c.ParentId == request.Id)
                .OrderBy(c => c.Name)
                .ProjectTo<ProductCategoryDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
            return categories;
        }
    }
}
