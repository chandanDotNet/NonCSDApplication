using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using POS.Common.UnitOfWork;
using POS.Data.Dto;
using POS.Domain;
using POS.Helper;
using POS.MediatR.Cart.Handlers;
using POS.MediatR.CommandAndQuery;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace POS.MediatR.Wishlist.Handlers
{
    public class AddWishlistCommandHandler : IRequestHandler<AddWishlistCommand, ServiceResponse<WishlistDto>>
    {

        private readonly IWishlistRepository _wishlistRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<AddWishlistCommandHandler> _logger;
        public AddWishlistCommandHandler(
           IWishlistRepository wishlistRepository,
            IMapper mapper,
            IUnitOfWork<POSDbContext> uow,
            ILogger<AddWishlistCommandHandler> logger
            )
        {
            _wishlistRepository = wishlistRepository;
            _mapper = mapper;
            _uow = uow;
            _logger = logger;
        }


        public async Task<ServiceResponse<WishlistDto>> Handle(AddWishlistCommand request, CancellationToken cancellationToken)
        {
            //var existingEntity = await _cartRepository.FindBy(c => c.CustomerName == request.CustomerName).FirstOrDefaultAsync();
            //if (existingEntity != null)
            //{
            //    _logger.LogError("Counter Name Already Exist");
            //    return ServiceResponse<CartDto>.Return409("Counter Name Already Exist.");
            //}

            var entity = _mapper.Map<Data.Wishlist>(request);
            //entity.Id = Guid.NewGuid();
            _wishlistRepository.Add(entity);
            if (await _uow.SaveAsync() <= 0)
            {

                _logger.LogError("Save Page have Error");
                return ServiceResponse<WishlistDto>.Return500();
            }
            return ServiceResponse<WishlistDto>.ReturnResultWith200(_mapper.Map<WishlistDto>(entity));
        }
    }
}
