using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using POS.Common.UnitOfWork;
using POS.Data.Dto;
using POS.Domain;
using POS.Helper;
using POS.MediatR.Tax.Commands;
using POS.Repository;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace POS.MediatR.Tax.Handlers
{
    public class AddTaxCommandHandler : IRequestHandler<AddTaxCommand, ServiceResponse<TaxDto>>
    {
        private readonly ITaxRepository _taxRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<AddTaxCommandHandler> _logger;
        public AddTaxCommandHandler(
           ITaxRepository taxRepository,
            IMapper mapper,
            IUnitOfWork<POSDbContext> uow,
            ILogger<AddTaxCommandHandler> logger
            )
        {
            _taxRepository = taxRepository;
            _mapper = mapper;
            _uow = uow;
            _logger = logger;
        }

        public async Task<ServiceResponse<TaxDto>> Handle(AddTaxCommand request, CancellationToken cancellationToken)
        {

            var existingEntity = await _taxRepository.FindBy(c => c.Name == request.Name).FirstOrDefaultAsync();
            if (existingEntity != null)
            {
                _logger.LogError("Tax Already Exist");
                return ServiceResponse<TaxDto>.Return409("Tax Already Exist.");
            }
            var entity = _mapper.Map<POS.Data.Tax>(request);
            entity.Id = Guid.NewGuid();
            _taxRepository.Add(entity);
            if (await _uow.SaveAsync() <= 0)
            {

                _logger.LogError("Save Page have Error");
                return ServiceResponse<TaxDto>.Return500();
            }
            return ServiceResponse<TaxDto>.ReturnResultWith200(_mapper.Map<TaxDto>(entity));
        }
    }
}
