using AutoMapper;
using POS.Data.Dto;
using POS.MediatR.CommandAndQuery;
using POS.Repository;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using POS.Helper;
using Microsoft.Extensions.Logging;

namespace POS.MediatR.Handlers
{
    public class GetPageQueryHandler : IRequestHandler<GetPageQuery, ServiceResponse<PageDto>>
    {
        private readonly IPageRepository _pageRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetPageQueryHandler> _logger;

        public GetPageQueryHandler(
            IPageRepository pageRepository,
            IMapper mapper,
            ILogger<GetPageQueryHandler> logger)
        {
            _pageRepository = pageRepository;
            _mapper = mapper;
            _logger = logger;

        }
        public async Task<ServiceResponse<PageDto>> Handle(GetPageQuery request, CancellationToken cancellationToken)
        {
            var entity = await _pageRepository.FindAsync(request.Id);
            if (entity != null)
                return ServiceResponse<PageDto>.ReturnResultWith200(_mapper.Map<PageDto>(entity));
            else
            {
                _logger.LogError("Not found");
                return ServiceResponse<PageDto>.Return404();
            }
        }
    }
}
