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
using System;
using System.Threading;
using System.Threading.Tasks;

namespace POS.MediatR.Country.Handlers
{
   public class AddCountryCommandHandler : IRequestHandler<AddCountryCommand, ServiceResponse<CountryDto>>
    {
        private readonly ICountryRepository _countryRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<AddCountryCommandHandler> _logger;
        public AddCountryCommandHandler(
           ICountryRepository countryRepository,
            IMapper mapper,
            IUnitOfWork<POSDbContext> uow,
            ILogger<AddCountryCommandHandler> logger
            )
        {
            _countryRepository = countryRepository;
            _mapper = mapper;
            _uow = uow;
            _logger = logger;
        }

        public async Task<ServiceResponse<CountryDto>> Handle(AddCountryCommand request, CancellationToken cancellationToken)
        {
            var existingEntity = await _countryRepository.FindBy(c => c.CountryName == request.CountryName).FirstOrDefaultAsync();
            if (existingEntity != null)
            {
                _logger.LogError("Country Name Already Exist");
                return ServiceResponse<CountryDto>.Return409("Country Name Already Exist.");
            }
            var entity = _mapper.Map<POS.Data.Country>(request);
            entity.Id = Guid.NewGuid();
            _countryRepository.Add(entity);
            if (await _uow.SaveAsync() <= 0)
            {

                _logger.LogError("Save Page have Error");
                return ServiceResponse<CountryDto>.Return500();
            }
            return ServiceResponse<CountryDto>.ReturnResultWith200(_mapper.Map<CountryDto>(entity));
        }
    }
}