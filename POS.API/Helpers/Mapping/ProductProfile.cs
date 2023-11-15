using AutoMapper;
using POS.Data;
using POS.Data.Dto;
using POS.MediatR.Product.Command;

namespace POS.API.Helpers.Mapping
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<ProductTax, ProductTaxDto>().ReverseMap();
            CreateMap<AddProductCommand, Product>();
            CreateMap<UpdateProductCommand, Product>();
        }
    }
}
