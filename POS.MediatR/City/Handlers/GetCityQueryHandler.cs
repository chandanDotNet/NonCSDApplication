using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using POS.Data.Dto;
using POS.Helper;
using POS.MediatR.City.Command;
using POS.Repository;
using System.Threading;
using System.Threading.Tasks;


namespace POS.MediatR.City.Handler
{
    public class GetCityQueryHandler : IRequestHandler<GetCityQuery, ServiceResponse<CityDto>>
    {
        private readonly ICityRepository _cityRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetCityQueryHandler> _logger;
        public GetCityQueryHandler(
           ICityRepository cityRepository,
            IMapper mapper,
            ILogger<GetCityQueryHandler> logger
            )
        {
            _cityRepository = cityRepository;
            _mapper = mapper;
            _logger = logger;
        }


        public async Task<ServiceResponse<CityDto>> Handle(GetCityQuery request, CancellationToken cancellationToken)
        {
            var entityDto = await _cityRepository.FindBy(c => c.Id == request.Id)
                .ProjectTo<CityDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
            if (entityDto == null)
            {
                _logger.LogError("City is not exists");
                return ServiceResponse<CityDto>.Return404();
            }
            return ServiceResponse<CityDto>.ReturnResultWith200(entityDto);
        }
    }
}
