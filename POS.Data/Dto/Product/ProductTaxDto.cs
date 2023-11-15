using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Data.Dto
{
    public class ProductTaxDto
    {
        public Guid? ProductId { get; set; }
        public Guid TaxId { get; set; }
    }
}
