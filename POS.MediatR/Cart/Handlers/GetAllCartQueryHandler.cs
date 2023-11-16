using MediatR;
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
    public class GetAllCartQueryHandler : IRequestHandler<GetAllCartQuery, CartList>
    {
        private readonly ICartRepository _cartRepository;

        public GetAllCartQueryHandler(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }
        public async Task<CartList> Handle(GetAllCartQuery request, CancellationToken cancellationToken)
        {
            return await _cartRepository.GetCartsData(request.CartResource);
        }

    }
}
