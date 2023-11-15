using AutoMapper;
using POS.Data;
using POS.Data.Dto;
using POS.Data.Entities;
using POS.MediatR.CommandAndQuery;
using POS.MediatR.Inventory.Command;

public class InventoryProfle : Profile
{
    public InventoryProfle()
    {
        CreateMap<Inventory, InventoryDto>().ReverseMap();
        CreateMap<InventoryHistory, InventoryHistoryDto>();
        CreateMap<AddInventoryCommand, InventoryDto>();
    }
}
