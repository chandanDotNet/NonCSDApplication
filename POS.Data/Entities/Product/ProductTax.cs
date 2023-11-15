using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Data
{
    public class ProductTax : BaseEntity
    {
        public Guid ProductId { get; set; }
        public Guid TaxId { get; set; }
        public Product Product { get; set; }
        public Tax Tax { get; set; }
    }
}
