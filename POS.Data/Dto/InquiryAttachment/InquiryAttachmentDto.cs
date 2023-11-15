using System;

namespace POS.Data.Dto
{
    public class InquiryAttachmentDto
    {
        public Guid Id { get; set; }
        public Guid InquiryId { get; set; }
        public string Path { get; set; }
        public string Name { get; set; }
        public UserDto CreatedByUser { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
