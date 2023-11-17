using MediatR;
using POS.Data.Dto;
using POS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.MediatR.PaymentCard.Commands
{
    public class AddPaymentCardCommand : IRequest<ServiceResponse<PaymentCardDto>>
    {
        public string NameOnCard { get; set; }
        public string CardNumber { get; set; }
        public string ExpiryDate { get; set; }
        public string CardType { get; set; }
        public string NickNameForCard { get; set; }
        public string CVV { get; set; }
        public Guid? CustomerId { get; set; }
        public bool IsPrimary { get; set; }
    }
}
