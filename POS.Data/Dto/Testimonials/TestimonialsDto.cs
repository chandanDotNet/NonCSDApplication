using System;

namespace POS.Data.Dto
{
    public class TestimonialsDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Designation { get; set; }
        public string Message { get; set; }
        public bool IsActive { get; set; }
        public string Url { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
