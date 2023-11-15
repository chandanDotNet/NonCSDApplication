using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using POS.Common.UnitOfWork;
using POS.Domain;
using POS.Helper;
using POS.MediatR.City.Commands;
using POS.Repository;
using System.Threading;
using System.Threading.Tasks;

namespace POS.MediatR.City.Handlers
{
    public class UpdateCityCommandHandler : IRequestHandler<UpdateCityCommand, ServiceResponse<bool>>
    {
        private readonly ICityRepository _cityRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateCityCommandHandler> _logger;
        public UpdateCityCommandHandler(
           ICityRepository cityRepository,
            IMapper mapper,
            IUnitOfWork<POSDbContext> uow,
            ILogger<UpdateCityCommandHandler> logger
            )
        {
            _cityRepository = cityRepository;
            _mapper = mapper;
            _uow = uow;
            _logger = logger;
        }
        public async Task<ServiceResponse<bool>> Handle(UpdateCityCommand request, CancellationToken cancellationToken)
        {
            var entityExist = await _cityRepository
                .All.FirstOrDefaultAsync(c => c.CityName == request.CityName && c.CountryId == request.CountryId && c.Id != request.Id);

            if (entityExist != null)
            {
                _logger.LogError("City Already Exist.");
                return ServiceResponse<bool>.Return409("City Already Exist.");
            }
            entityExist = await _cityRepository.FindBy(v => v.Id == request.Id).FirstOrDefaultAsync();
            entityExist.CityName = request.CityName;
            entityExist.CountryId = request.CountryId;
            _cityRepository.Update(entityExist);
            if (await _uow.SaveAsync() <= 0)
            {
                return ServiceResponse<bool>.Return500();
            }
            return ServiceResponse<bool>.ReturnResultWith200(true);
        }
    }
}
