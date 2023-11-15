using MediatR;
using POS.Data;
using POS.Data.Dto;
using POS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.MediatR.PurchaseOrderPayment.Command
{
    public class AddPurchaseOrderPaymentCommand : IRequest<ServiceResponse<PurchaseOrderPaymentDto>>
    {
        public Guid PurchaseOrderId { get; set; }
        public DateTime PaymentDate { get; set; }
        public string ReferenceNumber { get; set; }
        public decimal Amount { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public string Note { get; set; }
        public string AttachmentUrl { get; set; }
        public string AttachmentData { get; set; }
    }
}
