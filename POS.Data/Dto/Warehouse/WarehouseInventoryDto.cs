using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Data.Dto
{
    public class WarehouseInventoryDto
    {
        public Guid? Id { get; set; }
        public decimal Stock { get; set; }
        public Guid WarehouseId { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public string BaseUnit { get; set; }
        public string UnitCode { get; set; }

    }
}
