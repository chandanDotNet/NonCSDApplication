using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace POS.Data
{
    public class Supplier : BaseEntity
    {
        public Guid Id { get; set; }
        public string SupplierName { get; set; }
        public string ContactPerson { get; set; }
        public string Email { get; set; }
        public string Fax { get; set; }
        public string MobileNo { get; set; }
        public string PhoneNo { get; set; }
        public string Website { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public bool IsVarified { get; set; }
        public bool IsUnsubscribe { get; set; }
        public string SupplierProfile { get; set; }
        public string BusinessType { get; set; }
        public string SupplierNo { get; set; }
        public string SupplierType { get; set; }
        public Guid SupplierAddressId { get; set; }
        [ForeignKey("SupplierAddressId")]
        public SupplierAddress SupplierAddress { get; set; }
        public Guid BillingAddressId { get; set; }
        [ForeignKey("BillingAddressId")]
        public SupplierAddress BillingAddress { get; set; }
        public Guid ShippingAddressId { get; set; }
        [ForeignKey("ShippingAddressId")]
        public SupplierAddress ShippingAddress { get; set; }
        [ForeignKey("ModifiedBy")]
        public User ModifiedByUser { get; set; }
        [ForeignKey("DeletedBy")]
        public User DeletedByUser { get; set; }
    }
}
