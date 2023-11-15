using AutoMapper;
using POS.Data;
using POS.Data.Dto;
using POS.MediatR.CommandAndQuery;

namespace POS.API.Helpers
{
    /// <summary>
    /// Supplier Mapper for Autommaper
    /// </summary>
    public class SupplierProfile : Profile
    {
        /// <summary>
        /// Supplier Mapper for Autommaper
        /// </summary>
        public SupplierProfile()
        {
            CreateMap<SupplierAddressDto, SupplierAddress>().ReverseMap();
            CreateMap<Supplier, SupplierDto>()
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description == null ? string.Empty : src.Description)).ReverseMap();
            CreateMap<AddSupplierCommand, Supplier>();
            CreateMap<UpdateSupplierCommand, Supplier>();
        }
    }
}
