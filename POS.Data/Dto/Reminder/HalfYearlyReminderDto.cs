using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Data.Dto
{
    public class HalfYearlyReminderDto
    {
        public Guid? Id { get; set; }
        public Guid? ReminderId { get; set; }
        public int Day { get; set; }
        public int Month { get; set; }
        public QuarterEnum Quarter { get; set; }
    }
}
