using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Data.Dto
{
    public class DailyReminderDto
    {
        public Guid? Id { get; set; }
        public Guid? ReminderId { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
        public bool IsActive { get; set; }
    }
}
