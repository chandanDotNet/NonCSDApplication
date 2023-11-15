using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace POS.Data.Entities
{
    public class InquiryActivity : BaseEntity
    {
        public Guid Id { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public DateTime? DueDate { get; set; }
        public bool IsOpen { get; set; }
        public Guid? AssignTo { get; set; }
        public string Priority { get; set; }
        public Guid InquiryId { get; set; }
        [ForeignKey("InquiryId")]
        public Inquiry Inquiry { get; set; }
        [ForeignKey("AssignTo")]
        public User AssignUser { get; set; }
    }
}
