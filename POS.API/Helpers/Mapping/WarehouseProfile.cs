using AutoMapper;
using POS.Data;
using POS.Data.Dto;
using POS.MediatR.Warehouse.Commands;

namespace POS.API.Helpers.Mapping
{
    public class WarehouseProfile : Profile
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public WarehouseProfile()
        {
            CreateMap<Warehouse, WarehouseDto>().ReverseMap();
            CreateMap<AddWarehouseCommand, Warehouse>();
            CreateMap<UpdateWarehouseCommand, Warehouse>();
        }
    }
}
