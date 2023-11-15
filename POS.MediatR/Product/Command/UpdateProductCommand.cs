using MediatR;
using POS.Data.Dto;
using POS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.MediatR.Product.Command
{
    public class UpdateProductCommand : IRequest<ServiceResponse<ProductDto>>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Barcode { get; set; }
        public string SkuCode { get; set; }
        public string SkuName { get; set; }
        public string Description { get; set; }
        public string ProductUrlData { get; set; }
        public string QRCodeUrlData { get; set; }
        public Guid UnitId { get; set; }
        public Guid BrandId { get; set; }
        public Guid? WarehouseId { get; set; }

        public decimal? PurchasePrice { get; set; }
        public decimal? SalesPrice { get; set; }
        public decimal? Mrp { get; set; }
        public Guid CategoryId { get; set; }
        public bool IsProductImageUpload { get; set; }
        public bool IsQrCodeUpload { get; set; }
        public List<ProductTaxDto> ProductTaxes { get; set; } = new List<ProductTaxDto>();
    }
}
