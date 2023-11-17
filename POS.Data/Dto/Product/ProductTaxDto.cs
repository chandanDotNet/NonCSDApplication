using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Data.Dto
{
    public class ProductTaxDto
    {
        [Key]
        public Guid? ProductId { get; set; }
        public Guid TaxId { get; set; }
    }
}
