using System;

namespace POS.Data.Dto
{
    public class InquiryActivityDto
    {
        public Guid Id { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public DateTime? DueDate { get; set; }
        public bool IsOpen { get; set; }
        public Guid? AssignTo { get; set; }
        public string Priority { get; set; }
        public Guid InquiryId { get; set; }
        public UserDto AssignUser { get; set; }
    }
}
