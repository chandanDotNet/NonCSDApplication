using AutoMapper;
using POS.Data;
using POS.Data.Dto;
using POS.Data.Entities;
using POS.MediatR.CommandAndQuery;
using POS.MediatR.Inventory.Command;

namespace POS.API.Helpers.Mapping
{
    public class CartProfile : Profile
    {
        public CartProfile()
        {
            CreateMap<Cart, CartDto>().ReverseMap();
            CreateMap<AddCartCommand, Cart>();
            CreateMap<UpdateCartCommand, City>();
            CreateMap<DeleteCartCommand, City>();
        }
    }
}
