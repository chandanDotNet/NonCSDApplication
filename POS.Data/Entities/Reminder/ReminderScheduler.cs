using System;
using System.ComponentModel.DataAnnotations.Schema;


namespace POS.Data.Entities
{
    public class ReminderScheduler
    {
        public Guid Id { get; set; }
        public DateTime Duration { get; set; }
        public bool IsActive { get; set; } = true;
        public Frequency? Frequency { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
        public bool IsRead { get; set; }
        public bool IsEmailNotification { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public Guid? ReferenceId { get; set; }
        public ApplicationEnums? Application { get; set; }

    }
}
