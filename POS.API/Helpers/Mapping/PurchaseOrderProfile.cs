using AutoMapper;
using POS.Data.Dto;
using POS.Data;
using POS.MediatR.CommandAndQuery;
using POS.MediatR.PurchaseOrder.Commands;
using POS.MediatR.PurchaseOrderPayment.Command;

namespace POS.API.Helpers.Mapping
{
    public class PurchaseOrderProfile : Profile
    {
        public PurchaseOrderProfile()
        {
            CreateMap<PurchaseOrder, PurchaseOrderDto>().ReverseMap();
            CreateMap<AddPurchaseOrderCommand, PurchaseOrder>();
            CreateMap<PurchaseOrderItem, PurchaseOrderItemDto>().ReverseMap();
            CreateMap<PurchaseOrderItemTax, PurchaseOrderItemTaxDto>().ReverseMap();
            CreateMap<UpdatePurchaseOrderCommand, PurchaseOrder>();

            CreateMap<PurchaseOrderPayment, PurchaseOrderPaymentDto>().ReverseMap();
            CreateMap<AddPurchaseOrderPaymentCommand, PurchaseOrderPayment>();
            CreateMap<UpdatePurchaseOrderReturnCommand, PurchaseOrder>();
        }
    }
}
