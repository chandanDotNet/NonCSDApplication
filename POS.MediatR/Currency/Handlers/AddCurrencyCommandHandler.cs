using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using POS.Common.UnitOfWork;
using POS.Data.Dto;
using POS.Domain;
using POS.Helper;
using POS.MediatR.Currency.Commands;
using POS.Repository;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace POS.MediatR.Currency.Handlers
{
    public class AddCurrencyCommandHandler : IRequestHandler<AddCurrencyCommand, ServiceResponse<CurrencyDto>>
    {
        private readonly ICurrencyRepository _currencyRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<AddCurrencyCommandHandler> _logger;
        public AddCurrencyCommandHandler(
           ICurrencyRepository currencyRepository,
            IMapper mapper,
            IUnitOfWork<POSDbContext> uow,
            ILogger<AddCurrencyCommandHandler> logger
            )
        {
            _currencyRepository = currencyRepository;
            _mapper = mapper;
            _uow = uow;
            _logger = logger;
        }

        public async Task<ServiceResponse<CurrencyDto>> Handle(AddCurrencyCommand request, CancellationToken cancellationToken)
        {
            var existingEntity = await _currencyRepository.FindBy(c => c.Name == request.Name).FirstOrDefaultAsync();
            if (existingEntity != null)
            {
                _logger.LogError("Currency Already Exist");
                return ServiceResponse<CurrencyDto>.Return409("Currency Already Exist.");
            }
            var entity = _mapper.Map<POS.Data.Currency>(request);
            entity.Id = Guid.NewGuid();
            _currencyRepository.Add(entity);
            if (await _uow.SaveAsync() <= 0)
            {

                _logger.LogError("Save Page have Error");
                return ServiceResponse<CurrencyDto>.Return500();
            }
            return ServiceResponse<CurrencyDto>.ReturnResultWith200(_mapper.Map<CurrencyDto>(entity));
        }
    }
}
