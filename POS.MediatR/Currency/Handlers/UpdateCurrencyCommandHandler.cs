using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
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
   public class UpdateCurrencyCommandHandler : IRequestHandler<UpdateCurrencyCommand, ServiceResponse<bool>>
    {
        private readonly ICurrencyRepository _currencyRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateCurrencyCommandHandler> _logger;
        public UpdateCurrencyCommandHandler(
           ICurrencyRepository currencyRepository,
            IMapper mapper,
            IUnitOfWork<POSDbContext> uow,
            ILogger<UpdateCurrencyCommandHandler> logger
            )
        {
            _currencyRepository = currencyRepository;
            _mapper = mapper;
            _uow = uow;
            _logger = logger;
        }

        public async Task<ServiceResponse<bool>> Handle(UpdateCurrencyCommand request, CancellationToken cancellationToken)
        {
            var entityExist = await _currencyRepository.FindBy(c => c.Name == request.Name && c.Id != request.Id)
                .FirstOrDefaultAsync();
            if (entityExist != null)
            {
                _logger.LogError("Currency Already Exist.");
                return ServiceResponse<bool>.Return409("Currency Already Exist.");
            }
            entityExist = await _currencyRepository.FindBy(v => v.Id == request.Id).FirstOrDefaultAsync();
            entityExist.Name = request.Name;
            _currencyRepository.Update(entityExist);
            if (await _uow.SaveAsync() <= 0)
            {
                return ServiceResponse<bool>.Return500();
            }
            return ServiceResponse<bool>.ReturnResultWith200(true);
        }
    }
}