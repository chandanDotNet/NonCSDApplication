using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using POS.Common.UnitOfWork;
using POS.Data.Dto;
using POS.Domain;
using POS.Helper;
using POS.MediatR.Country.Commands;
using POS.Repository;
using System.Threading;
using System.Threading.Tasks;

namespace POS.MediatR.Country.Handlers
{
   public class UpdateCountryCommandHandler : IRequestHandler<UpdateCountryCommand, ServiceResponse<bool>>
    {
        private readonly ICountryRepository _countryRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateCountryCommandHandler> _logger;
        public UpdateCountryCommandHandler(
           ICountryRepository countryRepository,
            IMapper mapper,
            IUnitOfWork<POSDbContext> uow,
            ILogger<UpdateCountryCommandHandler> logger
            )
        {
            _countryRepository = countryRepository;
            _mapper = mapper;
            _uow = uow;
            _logger = logger;
        }

        public async Task<ServiceResponse<bool>> Handle(UpdateCountryCommand request, CancellationToken cancellationToken)
        {
            var entityExist = await _countryRepository.FindBy(c => c.CountryName == request.CountryName && c.Id != request.Id)
              .FirstOrDefaultAsync();
            if (entityExist != null)
            {
                _logger.LogError("Country Name Already Exist.");
                return ServiceResponse<bool>.Return409("Country Name Already Exist.");
            }
            entityExist = await _countryRepository.FindBy(v => v.Id == request.Id).FirstOrDefaultAsync();
            entityExist.CountryName = request.CountryName;
            _countryRepository.Update(entityExist);
            if (await _uow.SaveAsync() <= 0)
            {
                return ServiceResponse<bool>.Return500();
            }
            return ServiceResponse<bool>.ReturnResultWith200(true);
        }
    }
}
