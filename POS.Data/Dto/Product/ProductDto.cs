using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Data.Dto
{
    public class ProductDto
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Barcode { get; set; }
        public string SkuCode { get; set; }
        public string SkuName { get; set; }
        public string Description { get; set; }
        public string ProductUrl { get; set; }
        public string QRCodeUrl { get; set; }
        public Guid UnitId { get; set; }
        public decimal? PurchasePrice { get; set; }
        public decimal? SalesPrice { get; set; }
        public decimal? Mrp { get; set; }
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string UnitName { get; set; }
        public Guid BrandId { get; set; }
        public Guid? WarehouseId { get; set; }
        public string WarehouseName { get; set; }
        public string BrandName { get; set; }
        public UnitConversationDto Unit { get; set; }
        public List<ProductTaxDto> ProductTaxes { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
