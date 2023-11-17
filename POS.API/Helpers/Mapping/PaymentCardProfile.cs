using AutoMapper;
using POS.Data.Dto;
using POS.Data;
using POS.MediatR.CustomerAddress.Commands;
using POS.MediatR.PaymentCard.Commands;

namespace POS.API.Helpers.Mapping
{
    public class PaymentCardProfile : Profile
    {
        public PaymentCardProfile()
        {
            CreateMap<PaymentCard, PaymentCardDto>().ReverseMap();
            CreateMap<AddPaymentCardCommand, PaymentCard>();
            //CreateMap<UpdatePaymentCardCommand, PaymentCard>();
        }
    }
}
