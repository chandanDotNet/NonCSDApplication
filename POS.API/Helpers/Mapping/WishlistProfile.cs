using POS.Data.Dto;
using POS.Data;
using POS.MediatR.CommandAndQuery;
using AutoMapper;

namespace POS.API.Helpers.Mapping
{
    public class WishlistProfile : Profile
    {
        public WishlistProfile()
        {
            CreateMap<Wishlist, WishlistDto>().ReverseMap();
            CreateMap<AddWishlistCommand, Wishlist>();
            //CreateMap<UpdateWishlistCommand, Wishlist>();
            //CreateMap<DeleteWishlistCommand, Wishlist>();
        }
    }
}
