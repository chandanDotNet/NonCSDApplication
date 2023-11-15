using POS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Data.Resources
{
    public class ProductResource : ResourceParameters
    {
        public ProductResource() : base("CreatedDate")
        {
        }

        public string Name { get; set; }
        public string Barcode { get; set; }
        public Guid? UnitId { get; set; }
        public Guid? CategoryId { get; set; }
        public Guid? BrandId { get; set; }
        public Guid? WarehouseId { get; set; }
    }
}
