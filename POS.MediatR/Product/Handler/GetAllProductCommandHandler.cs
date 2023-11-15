using MediatR;
using POS.MediatR.Product.Command;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace POS.MediatR.Product.Handler
{
    public class GetAllProductCommandHandler : IRequestHandler<GetAllProductCommand, ProductList>
    {
        private readonly IProductRepository _productRepository;
        public GetAllProductCommandHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        public async Task<ProductList> Handle(GetAllProductCommand request, CancellationToken cancellationToken)
        {
            return await _productRepository.GetProducts(request.ProductResource);
        }
    }
}
