using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using POS.Common.UnitOfWork;
using POS.Domain;
using POS.Helper;
using POS.MediatR.Cart;
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
    public class DeleteWishlistCommandHandler : IRequestHandler<DeleteWishlistCommand, ServiceResponse<bool>>
    {
        private readonly IWishlistRepository _wishlistRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<DeleteWishlistCommandHandler> _logger;
        public DeleteWishlistCommandHandler(
           IWishlistRepository wishlistRepository,
            IMapper mapper,
            IUnitOfWork<POSDbContext> uow,
            ILogger<DeleteWishlistCommandHandler> logger
            )
        {
            _wishlistRepository = wishlistRepository;
            _mapper = mapper;
            _uow = uow;
            _logger = logger;
        }

        public async Task<ServiceResponse<bool>> Handle(DeleteWishlistCommand request, CancellationToken cancellationToken)
        {
            var existingWishlist = await _wishlistRepository.FindAsync(request.Id);
            if (existingWishlist == null)
            {
                _logger.LogError("Data does not exists.");
                return ServiceResponse<bool>.Return404("Data does not exists.");
            }
            _wishlistRepository.Delete(existingWishlist);

            if (await _uow.SaveAsync() <= 0)
            {
                _logger.LogError("Error While deleting Cart.");
                return ServiceResponse<bool>.Return500();
            }

            return ServiceResponse<bool>.ReturnSuccess();
        }
    }
}
