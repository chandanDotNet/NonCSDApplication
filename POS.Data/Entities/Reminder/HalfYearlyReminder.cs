using POS.Data.Dto;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace POS.Data
{
    public class HalfYearlyReminder
    {
        public Guid Id { get; set; }
        public Guid ReminderId { get; set; }
        [ForeignKey("ReminderId")]
        public Reminder Reminder { get; set; }
        public int Day { get; set; }
        public int Month { get; set; }
        public QuarterEnum Quarter { get; set; }
    }
}
