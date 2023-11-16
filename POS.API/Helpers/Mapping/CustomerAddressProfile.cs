using AutoMapper;
using POS.Data.Dto;
using POS.Data;
using POS.MediatR.CustomerAddress.Commands;

namespace POS.API.Helpers.Mapping
{
    public class CustomerAddressProfile : Profile
    {
        public CustomerAddressProfile()
        {
            CreateMap<CustomerAddress, CustomerAddressDto>().ReverseMap();
            CreateMap<AddCustomerAddressCommand, CustomerAddress>();
            CreateMap<UpdateCustomerAddressCommand, CustomerAddress>();
        }
    }
}
