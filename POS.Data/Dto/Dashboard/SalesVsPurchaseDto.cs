using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Data.Dto
{
    public class SalesVsPurchaseDto
    {
        public DateTime Date { get; set; }
        public decimal TotalPurchase { get; set; }
        public decimal TotalSales { get; set; }
    }
}
