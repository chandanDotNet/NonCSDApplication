using System;

namespace POS.Data.Dto
{
    public class InquiryNoteDto
    {
        public Guid Id { get; set; }
        public Guid InquiryId { get; set; }
        public string Note { get; set; }
        public Guid? CreatedBy { get; set; }
        public UserDto CreatedByUser { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
