using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using POS.Common.UnitOfWork;
using POS.Data.Dto;
using POS.Domain;
using POS.Helper;
using POS.MediatR.City.Commands;
using POS.Repository;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace POS.MediatR.City.Handlers
{
    public class AddCityCommandHandler : IRequestHandler<AddCityCommand, ServiceResponse<CityDto>>
    {
        private readonly ICityRepository _cityRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<AddCityCommandHandler> _logger;
        public AddCityCommandHandler(
           ICityRepository cityRepository,
            IMapper mapper,
            IUnitOfWork<POSDbContext> uow,
            ILogger<AddCityCommandHandler> logger
            )
        {
            _cityRepository = cityRepository;
            _mapper = mapper;
            _uow = uow;
            _logger = logger;
        }

        public async Task<ServiceResponse<CityDto>> Handle(AddCityCommand request, CancellationToken cancellationToken)
        {
            var existingEntity = await _cityRepository
                .All.FirstOrDefaultAsync(c => c.CityName == request.CityName && request.CountryId == c.CountryId);
            if (existingEntity != null)
            {
                _logger.LogError("City Already Exist");
                return ServiceResponse<CityDto>.Return409("City Already Exist.");
            }
            var entity = _mapper.Map<POS.Data.City>(request);
            entity.Id = Guid.NewGuid();
            _cityRepository.Add(entity);
            if (await _uow.SaveAsync() <= 0)
            {

                _logger.LogError("Save Page have Error");
                return ServiceResponse<CityDto>.Return500();
            }
            return ServiceResponse<CityDto>.ReturnResultWith200(_mapper.Map<CityDto>(entity));
        }
    }
}
