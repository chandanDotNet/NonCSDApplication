using System;

namespace POS.Data.Dto
{
    public class SupplierAddressDto
    {
        public Guid? Id { get; set; }
        public Guid? SupplierId { get; set; }
        public string Address { get; set; }
        public Guid? CityId { get; set; }
        public Guid? CountryId { get; set; }
        public string CityName { get; set; }
        public string CountryName { get; set; }
    }
}
