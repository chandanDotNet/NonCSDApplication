using MediatR;
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
    public class GetAllWishlistQueryHandler : IRequestHandler<GetAllWishlistQuery, AllWishList>
    {
        private readonly IWishlistRepository _wishlistRepository;

        public GetAllWishlistQueryHandler(IWishlistRepository wishlistRepository)
        {
            _wishlistRepository = wishlistRepository;
        }
        public async Task<AllWishList> Handle(GetAllWishlistQuery request, CancellationToken cancellationToken)
        {
            return await _wishlistRepository.GetWishlistData(request.WishlistResource);
        }

    }
}
