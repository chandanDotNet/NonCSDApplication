using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace POS.Data
{
    public class DailyReminder
    {
        public Guid Id { get; set; }
        public Guid ReminderId { get; set; }
        [ForeignKey("ReminderId")]
        public Reminder Reminder { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
        public bool IsActive { get; set; }

    }
}
