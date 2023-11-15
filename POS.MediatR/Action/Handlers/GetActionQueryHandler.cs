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
    public class GetActionQueryHandler : IRequestHandler<GetActionQuery, ServiceResponse<ActionDto>>
    {
        private readonly IActionRepository _actionRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetActionQueryHandler> _logger;

        public GetActionQueryHandler(
           IActionRepository actionRepository,
           IMapper mapper,
           ILogger<GetActionQueryHandler> logger)
        {
            _actionRepository = actionRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ServiceResponse<ActionDto>> Handle(GetActionQuery request, CancellationToken cancellationToken)
        {
            var entity = await _actionRepository.FindAsync(request.Id);
            if (entity != null)
            {
                return ServiceResponse<ActionDto>.ReturnResultWith200(_mapper.Map<ActionDto>(entity));
            }
            else
            {
                _logger.LogError("Action not found");
                return ServiceResponse<ActionDto>.Return404();
            }
        }
    }
}
