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
    public class GetCitiesByContryIdQueryHandler : IRequestHandler<GetCitiesByContryIdQuery, List<CityDto>>
    {

        private readonly ICityRepository _cityRepository;

        private readonly IMapper _mapper;

        public GetCitiesByContryIdQueryHandler(ICityRepository cityRepository,
            IMapper mapper)
        {
            _cityRepository = cityRepository;
            _mapper = mapper;
        }

        public async Task<List<CityDto>> Handle(GetCitiesByContryIdQuery request, CancellationToken cancellationToken)
        {
            return await _cityRepository.All
                .Where(c => c.CountryId == request.CountryId || EF.Functions.Like(c.CityName, $"{request.CityName}%"))
                .Take(10)
                .ProjectTo<CityDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }
    }
}
