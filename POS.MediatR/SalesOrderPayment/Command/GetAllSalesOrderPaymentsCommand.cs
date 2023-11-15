using MediatR;
using POS.Data.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.MediatR.SalesOrderPayment.Command
{
    public class GetAllSalesOrderPaymentsCommand : IRequest<List<SalesOrderPaymentDto>>
    {
        public Guid Id { get; set; }
    }

}
