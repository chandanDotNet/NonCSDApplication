using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using POS.Data.Dto;
using POS.Helper;
using POS.MediatR.Counter.Commands;
using POS.MediatR.Counter.Handlers;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace POS.MediatR.Counter.Handlers
{
    public class GetCounterCommandHandler : IRequestHandler<GetCounterCommand, ServiceResponse<CounterDto>>
    {
        private readonly ICounterRepository _counterRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetCounterCommandHandler> _logger;
        public GetCounterCommandHandler(
           ICounterRepository counterRepository,
            IMapper mapper,
            ILogger<GetCounterCommandHandler> logger
            )
        {
            _counterRepository = counterRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ServiceResponse<CounterDto>> Handle(GetCounterCommand request, CancellationToken cancellationToken)
        {
            var entityDto = await _counterRepository.FindBy(c => c.Id == request.Id)
                .ProjectTo<CounterDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
            if (entityDto == null)
            {
                _logger.LogError("Counter is not exists");
                return ServiceResponse<CounterDto>.Return404();
            }
            return ServiceResponse<CounterDto>.ReturnResultWith200(entityDto);
        }
    }
}