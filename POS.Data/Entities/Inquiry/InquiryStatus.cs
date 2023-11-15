using System;

namespace POS.Data.Entities
{
    public class InquiryStatus : BaseEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
