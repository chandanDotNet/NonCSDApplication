using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using POS.Common.UnitOfWork;
using POS.Data;
using POS.Data.Dto;
using POS.Domain;
using POS.Helper;
using POS.MediatR.CommandAndQuery;
using POS.Repository;

namespace POS.MediatR.Handlers
{
    public class AddLogCommandHandler : IRequestHandler<AddLogCommand, ServiceResponse<NLogDto>>
    {
        private readonly INLogRepository _nLogRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        public AddLogCommandHandler(
           INLogRepository nLogRepository,
            IUnitOfWork<POSDbContext> uow
            )
        {
            _nLogRepository = nLogRepository;
            _uow = uow;
        }
        public async Task<ServiceResponse<NLogDto>> Handle(AddLogCommand request, CancellationToken cancellationToken)
        {
            _nLogRepository.Add(new NLog
            {
                Id = Guid.NewGuid(),
                Logged = DateTime.UtcNow,
                Level = "Error",
                Message = request.ErrorMessage,
                Source = "Angular",
                Exception = request.Stack
            });
            await _uow.SaveAsync();
            return ServiceResponse<NLogDto>.ReturnSuccess();
        }
    }
}
