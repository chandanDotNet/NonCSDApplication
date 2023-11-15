using MediatR;
using POS.Data.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.MediatR.PurchaseOrderPayment.Command
{
    public class GetAllPurchaseOrderPaymentsCommand : IRequest<List<PurchaseOrderPaymentDto>>
    {
        public Guid Id { get; set; }
    }
}
