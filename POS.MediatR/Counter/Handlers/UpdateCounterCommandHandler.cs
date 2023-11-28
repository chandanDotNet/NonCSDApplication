using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using POS.Common.UnitOfWork;
using POS.Data.Dto;
using POS.Domain;
using POS.Helper;
using POS.MediatR.Counter.Commands;
using POS.MediatR.Counter.Handlers;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace POS.MediatR.Counter.Handlers
{
    public class UpdateCounterCommandHandler : IRequestHandler<UpdateCounterCommand, ServiceResponse<CounterDto>>
    {
        private readonly ICounterRepository _counterRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly ILogger<UpdateCounterCommandHandler> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly PathHelper _pathHelper;
        private readonly IMapper _mapper;

        public UpdateCounterCommandHandler(
          ICounterRepository counterRepository,
           IUnitOfWork<POSDbContext> uow,
           ILogger<UpdateCounterCommandHandler> logger,
           IWebHostEnvironment webHostEnvironment,
           PathHelper pathHelper,
           IMapper mapper
           )
        {
            _counterRepository = counterRepository;
            _uow = uow;
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
            _pathHelper = pathHelper;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<CounterDto>> Handle(UpdateCounterCommand request, CancellationToken cancellationToken)
        {
            var entityExist = await _counterRepository.FindBy(c => c.CounterName == request.CounterName && c.Id != request.Id)
             .FirstOrDefaultAsync();
            if (entityExist != null)
            {
                _logger.LogError("Brand Already Exist.");
                return ServiceResponse<CounterDto>.Return409("Brand Already Exist.");
            }
            entityExist = await _counterRepository.FindBy(v => v.Id == request.Id).FirstOrDefaultAsync();
            entityExist.CounterName = request.CounterName;
            //entityExist.EndPoint = request.EndPoint;

            _counterRepository.Update(entityExist);

            if (await _uow.SaveAsync() <= 0)
            {
                return ServiceResponse<CounterDto>.Return500();
            }

            
            var result = _mapper.Map<CounterDto>(entityExist);
           
            return ServiceResponse<CounterDto>.ReturnResultWith200(result);
        }
    }
}
