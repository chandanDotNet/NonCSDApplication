using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using POS.Data.Dto;
using POS.MediatR.Counter.Commands;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace POS.MediatR.Counter.Handlers
{
    public class GetAllCounterCommandHandler : IRequestHandler<GetAllCounterCommand, List<CounterDto>>
    {
        private readonly ICounterRepository _counterRepository;
        private readonly IMapper _mapper;

        public GetAllCounterCommandHandler(
           ICounterRepository counterRepository,
            IMapper mapper)
        {
            _counterRepository = counterRepository;
            _mapper = mapper;
        }

        public async Task<List<CounterDto>> Handle(GetAllCounterCommand request, CancellationToken cancellationToken)
        {
            var entities = await _counterRepository.All
                .Select(c => new CounterDto
                {
                    Id = c.Id,
                    CounterName = c.CounterName,
                }).ToListAsync();
            return entities;
        }
    }
}
