using POS.Data.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Data
{
    public class CartResource : ResourceParameter
    {
        public CartResource() : base("CustomerName")
        {
        }

        
        public string CustomerName { get; set; }
        public string ProductName { get; set; }
        public Guid CustomerId { get; set; }       
        public Guid? ProductId { get; set; }
       // public SalesOrderStatus Status { get; set; } = SalesOrderStatus.All;
        

    }
}
