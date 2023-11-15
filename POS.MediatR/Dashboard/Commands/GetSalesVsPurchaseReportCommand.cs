using MediatR;
using POS.Data.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.MediatR.Dashboard.Commands
{
    public class GetSalesVsPurchaseReportCommand : IRequest<List<SalesVsPurchaseDto>>
    {
        public int Month { get; set; }
        public int Year { get; set; }
    }
}
