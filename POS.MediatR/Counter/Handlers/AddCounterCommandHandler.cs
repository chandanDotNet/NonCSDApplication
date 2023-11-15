using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using POS.Common.UnitOfWork;
using POS.Data.Dto;
using POS.Domain;
using POS.Helper;
using POS.MediatR.Counter.Commands;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace POS.MediatR.Counter.Handlers
{
    public class AddCounterCommandHandler : IRequestHandler<AddCounterCommand, ServiceResponse<CounterDto>>
    {
        private readonly ICounterRepository _counterRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<AddCounterCommandHandler> _logger;
        public AddCounterCommandHandler(
           ICounterRepository counterRepository,
            IMapper mapper,
            IUnitOfWork<POSDbContext> uow,
            ILogger<AddCounterCommandHandler> logger
            )
        {
            _counterRepository = counterRepository;
            _mapper = mapper;
            _uow = uow;
            _logger = logger;
        }

        public async Task<ServiceResponse<CounterDto>> Handle(AddCounterCommand request, CancellationToken cancellationToken)
        {
            var existingEntity = await _counterRepository.FindBy(c => c.CounterName == request.CounterName).FirstOrDefaultAsync();
            if (existingEntity != null)
            {
                _logger.LogError("Counter Name Already Exist");
                return ServiceResponse<CounterDto>.Return409("Counter Name Already Exist.");
            }
            var entity = _mapper.Map<Data.Counter>(request);
            //entity.Id = Guid.NewGuid();
            _counterRepository.Add(entity);
            if (await _uow.SaveAsync() <= 0)
            {

                _logger.LogError("Save Page have Error");
                return ServiceResponse<CounterDto>.Return500();
            }
            return ServiceResponse<CounterDto>.ReturnResultWith200(_mapper.Map<CounterDto>(entity));
        }
    }
}
