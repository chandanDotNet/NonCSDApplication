using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using POS.Common.UnitOfWork;
using POS.Data.Dto;
using POS.Domain;
using POS.Helper;
using POS.MediatR.CommandAndQuery;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace POS.MediatR.Cart.Handlers
{
    public class UpdateCartCommandHandler : IRequestHandler<UpdateCartCommand, ServiceResponse<CartDto>>
    {
        private readonly ICartRepository _cartRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateCartCommandHandler> _logger;
        public UpdateCartCommandHandler(
           ICartRepository cartRepository,
            IMapper mapper,
            IUnitOfWork<POSDbContext> uow,
            ILogger<UpdateCartCommandHandler> logger
            )
        {
            _cartRepository = cartRepository;
            _mapper = mapper;
            _uow = uow;
            _logger = logger;
        }

        public async Task<ServiceResponse<CartDto>> Handle(UpdateCartCommand request, CancellationToken cancellationToken)
        {
            var cart = await _cartRepository.FindAsync(request.Id);
            if (cart == null)
            {
                _logger.LogError("Customer does not exists.");
                return ServiceResponse<CartDto>.Return404();
            }

            //var entity = _mapper.Map<Data.Cart>(request);
            //entity.Id = Guid.NewGuid();
            //_cartRepository.Add(entity);
            cart.CustomerId = request.CustomerId;
            cart.CustomerName= request.CustomerName;
            cart.ProductId = request.ProductId;
            cart.ProductName = request.ProductName;
            cart.Quantity = request.Quantity;
            cart.UnitPrice = request.UnitPrice;
            cart.UnitName = request.UnitName;
            cart.UnitId = request.UnitId;
            cart.Total = request.Total;
            cart.TaxValue = request.TaxValue;
            cart.Discount = request.Discount;
            cart.DiscountPercentage = request.DiscountPercentage;

            //var updateCart = _mapper.Map(request, cart);
            _cartRepository.Update(cart);
            if (await _uow.SaveAsync() <= 0)
            {
                _logger.LogError("Save Page have Error");
                return ServiceResponse<CartDto>.Return500();
            }
            return ServiceResponse<CartDto>.ReturnResultWith200(_mapper.Map<CartDto>(cart));
        }
    }
}
