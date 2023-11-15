using System;

namespace POS.Data.Dto
{
    public class InquiryProductDto
    {
        public Guid ProductId { get; set; }
        public Guid? InquiryId { get; set; }
        public string Name { get; set; }
        public InquiryDto Inquiry { get; set; } 
    }
}