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
using AutoMapper.QueryableExtensions;

namespace POS.MediatR.Handlers
{
    public class GetCountryQueryHandler : IRequestHandler<GetCountryQuery, List<CountryDto>>
    {
        private readonly ICountryRepository _countryRepository;
        private readonly IMapper _mapper;

        public GetCountryQueryHandler(ICountryRepository countryRepository,
            IMapper mapper)
        {
            _countryRepository = countryRepository;
            _mapper = mapper;
        }
        public async Task<List<CountryDto>> Handle(GetCountryQuery request, CancellationToken cancellationToken)
        {
            return await _countryRepository.All
                .ProjectTo<CountryDto>(_mapper.ConfigurationProvider)
                .OrderBy(c => c.CountryName).ToListAsync();
        }
    }
}
