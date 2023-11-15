using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace POS.Data
{
    public class Testimonials : BaseEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Designation { get; set; }
        public string Message { get; set; }
        public bool IsActive { get; set; }
        public string Url { get; set; }
        [ForeignKey("ModifiedBy")]
        public User ModifiedByUser { get; set; }
        [ForeignKey("DeletedBy")]
        public User DeletedByUser { get; set; }
    }
}
