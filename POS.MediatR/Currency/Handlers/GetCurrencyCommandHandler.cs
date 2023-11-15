using AutoMapper;
using MediatR;
using POS.Data.Dto;
using POS.Helper;
using POS.MediatR.Currency.Commands;
using POS.Repository;
using System.Threading;
using System.Threading.Tasks;

namespace POS.MediatR.Currency.Handlers
{
    public class GetCurrencyCommandHandler : IRequestHandler<GetCurrencyCommand, ServiceResponse<CurrencyDto>>
    {
        private readonly ICurrencyRepository _currencyRepository;
        private readonly IMapper _mapper;
        public GetCurrencyCommandHandler(
           ICurrencyRepository currencyRepository,
            IMapper mapper
            )
        {
            _currencyRepository = currencyRepository;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<CurrencyDto>> Handle(GetCurrencyCommand request, CancellationToken cancellationToken)
        {
            var entity = await _currencyRepository.FindAsync(request.Id);
            if (entity != null)
                return ServiceResponse<CurrencyDto>.ReturnResultWith200(_mapper.Map<CurrencyDto>(entity));
            else
            {
                return ServiceResponse<CurrencyDto>.Return404();
            }
        }
    }
}
