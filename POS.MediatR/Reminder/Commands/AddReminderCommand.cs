using POS.Data.Dto;
using POS.Data.Entities;
using POS.Helper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.MediatR.CommandAndQuery
{
    public class AddReminderCommand : IRequest<ServiceResponse<ReminderDto>>
    {
        public Guid? Id { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public Frequency? Frequency { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DayOfWeek? DayOfWeek { get; set; }
        public bool IsRepeated { get; set; }
        public bool IsEmailNotification { get; set; }
        public ICollection<ReminderUserDto> ReminderUsers { get; set; }
        public ICollection<DailyReminderDto> DailyReminders { get; set; }
        public ICollection<QuarterlyReminderDto> QuarterlyReminders { get; set; }
        public ICollection<HalfYearlyReminderDto> HalfYearlyReminders { get; set; }
    }
}
