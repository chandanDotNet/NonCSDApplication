using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using POS.Data.Dto;
using POS.MediatR.Currency.Commands;
using POS.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace POS.MediatR.Currency.Handlers
{
    public class GetAllCurrencyCommandHandler : IRequestHandler<GetAllCurrencyCommand, List<CurrencyDto>>
    {
        private readonly ICurrencyRepository _currencyRepository;
        private readonly IMapper _mapper;
        public GetAllCurrencyCommandHandler(
           ICurrencyRepository currencyRepository,
            IMapper mapper
            )
        {
            _currencyRepository = currencyRepository;
            _mapper = mapper;
        }

        public async Task<List<CurrencyDto>> Handle(GetAllCurrencyCommand request, CancellationToken cancellationToken)
        {
            return await _currencyRepository.All.OrderBy(c => c.Name).ProjectTo<CurrencyDto>(_mapper.ConfigurationProvider).ToListAsync();
        }
    }
}
