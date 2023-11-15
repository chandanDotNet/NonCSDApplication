using MediatR;
using POS.MediatR.CommandAndQuery;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace POS.MediatR.City.Handlers
{
    public class GetAllCityQueryHandler : IRequestHandler<GetAllCityQuery, CityList>
    {
        private readonly ICityRepository _cityRepository;
        public GetAllCityQueryHandler(ICityRepository cityRepository)
        {
            _cityRepository = cityRepository;
        }
        public async Task<CityList> Handle(GetAllCityQuery request, CancellationToken cancellationToken)
        {
            return await _cityRepository.GetCities(request.CityResource);
        }
    }
}
