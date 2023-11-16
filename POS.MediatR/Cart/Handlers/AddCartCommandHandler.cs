using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using POS.Common.UnitOfWork;
using POS.Data.Dto;

using POS.Domain;
using POS.Helper;

using POS.MediatR.CommandAndQuery;
using POS.MediatR.Counter.Commands;
using POS.MediatR.Counter.Handlers;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace POS.MediatR.Cart.Handlers
{
    public class AddCartCommandHandler : IRequestHandler<AddCartCommand, ServiceResponse<CartDto>>
    {

        private readonly ICartRepository _cartRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<AddCartCommandHandler> _logger;
        public AddCartCommandHandler(
           ICartRepository cartRepository,
            IMapper mapper,
            IUnitOfWork<POSDbContext> uow,
            ILogger<AddCartCommandHandler> logger
            )
        {
            _cartRepository = cartRepository;
            _mapper = mapper;
            _uow = uow;
            _logger = logger;
        }


        public async Task<ServiceResponse<CartDto>> Handle(AddCartCommand request, CancellationToken cancellationToken)
        {
            //var existingEntity = await _cartRepository.FindBy(c => c.CustomerName == request.CustomerName).FirstOrDefaultAsync();
            //if (existingEntity != null)
            //{
            //    _logger.LogError("Counter Name Already Exist");
            //    return ServiceResponse<CartDto>.Return409("Counter Name Already Exist.");
            //}

            var entity = _mapper.Map<Data.Cart>(request);
            //entity.Id = Guid.NewGuid();
            _cartRepository.Add(entity);
            if (await _uow.SaveAsync() <= 0)
            {

                _logger.LogError("Save Page have Error");
                return ServiceResponse<CartDto>.Return500();
            }
            return ServiceResponse<CartDto>.ReturnResultWith200(_mapper.Map<CartDto>(entity));
        }
    }
}
