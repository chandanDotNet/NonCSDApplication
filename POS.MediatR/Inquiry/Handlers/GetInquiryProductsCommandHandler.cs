using MediatR;
using Microsoft.EntityFrameworkCore;
using POS.Data.Dto;
using POS.MediatR.Inquiry.Commands;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace POS.MediatR.Inquiry.Handlers
{
    public class GetInquiryProductsCommandHandler : IRequestHandler<GetInquiryProductsCommand, List<ProductDto>>
    {
        private readonly IInquiryProductRepository _inquiryProductRepository;

        public GetInquiryProductsCommandHandler(IInquiryProductRepository inquiryProductRepository)
        {
            _inquiryProductRepository = inquiryProductRepository;
        }
        public async Task<List<ProductDto>> Handle(GetInquiryProductsCommand request, CancellationToken cancellationToken)
        {
            var products = await _inquiryProductRepository
                .AllIncluding(c => c.Product.Brand, cs => cs.Product.ProductCategory)
                .Where(c => c.InquiryId == request.Id)
                .Select(c => new ProductDto
                {
                    Name = c.Product.Name,
                    Id = c.Product.Id,
                    BrandName = c.Product.Brand.Name,
                    CategoryName = c.Product.ProductCategory.Name,
                    PurchasePrice = c.Product.PurchasePrice,
                    SalesPrice = c.Product.SalesPrice,
                    Mrp = c.Product.Mrp
                }).ToListAsync();
            return products;
        }
    }
}
