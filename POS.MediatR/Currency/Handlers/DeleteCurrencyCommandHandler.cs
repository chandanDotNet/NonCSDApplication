using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using POS.Common.UnitOfWork;
using POS.Domain;
using POS.Helper;
using POS.MediatR.Currency.Commands;
using POS.Repository;
using System.Threading;
using System.Threading.Tasks;

namespace POS.MediatR.Currency.Handlers
{
    public class DeleteCurrencyCommandHandler : IRequestHandler<DeleteCurrencyCommand, ServiceResponse<bool>>
    {
        private readonly ICurrencyRepository _currencyRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<DeleteCurrencyCommandHandler> _logger;
        public DeleteCurrencyCommandHandler(
           ICurrencyRepository currencyRepository,
            IMapper mapper,
            IUnitOfWork<POSDbContext> uow,
            ILogger<DeleteCurrencyCommandHandler> logger
            )
        {
            _currencyRepository = currencyRepository;
            _mapper = mapper;
            _uow = uow;
            _logger = logger;
        }

        public async Task<ServiceResponse<bool>> Handle(DeleteCurrencyCommand request, CancellationToken cancellationToken)
        {
            var entityExist = await _currencyRepository.FindAsync(request.Id);
            if (entityExist == null)
            {
                return ServiceResponse<bool>.Return404();
            }
            _currencyRepository.Delete(request.Id);
            if (await _uow.SaveAsync() <= 0)
            {
                return ServiceResponse<bool>.Return500();
            }
            return ServiceResponse<bool>.ReturnResultWith200(true);
        }
    }
}
