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

namespace POS.MediatR.Handlers
{
    public class GetAllPageQueryHandler : IRequestHandler<GetAllPageQuery, List<PageDto>>
    {
        private readonly IPageRepository _pageRepository;
        private readonly IMapper _mapper;

        public GetAllPageQueryHandler(
            IPageRepository pageRepository,
            IMapper mapper)
        {
            _pageRepository = pageRepository;
            _mapper = mapper;

        }
        public async Task<List<PageDto>> Handle(GetAllPageQuery request, CancellationToken cancellationToken)
        {
            var entities = await _pageRepository.All.OrderBy(c => c.Order).ToListAsync();
            return _mapper.Map<List<PageDto>>(entities);
        }
    }
}
