using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using POS.Data.Dto;
using POS.Helper;
using POS.MediatR.CommandAndQuery;
using POS.Repository;

namespace POS.MediatR.Handlers
{
    class GetLogQueryHandler : IRequestHandler<GetLogQuery, ServiceResponse<NLogDto>>
    {
        private readonly INLogRepository _nLogRepository;
        private readonly IMapper _mapper;

        public GetLogQueryHandler(
           INLogRepository nLogRepository,
           IMapper mapper)
        {
            _nLogRepository = nLogRepository;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<NLogDto>> Handle(GetLogQuery request, CancellationToken cancellationToken)
        {
            var entity = await _nLogRepository.FindAsync(request.Id);
            if (entity != null)
                return ServiceResponse<NLogDto>.ReturnResultWith200(_mapper.Map<NLogDto>(entity));
            else
                return ServiceResponse<NLogDto>.Return404();
        }
    }
}
