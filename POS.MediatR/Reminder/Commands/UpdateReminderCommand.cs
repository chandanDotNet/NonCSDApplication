using POS.Data.Dto;
using POS.Data.Entities;
using POS.Helper;
using MediatR;
using System;
using System.Collections.Generic;

namespace POS.MediatR.CommandAndQuery
{
    public class UpdateReminderCommand : IRequest<ServiceResponse<ReminderDto>>
    {
        public Guid Id { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public Frequency? Frequency { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DayOfWeek? DayOfWeek { get; set; }
        public bool IsRepeated { get; set; }
        public bool IsEmailNotification { get; set; }
        public List<ReminderUserDto> ReminderUsers { get; set; } = new List<ReminderUserDto>();
        public List<DailyReminderDto> DailyReminders { get; set; } = new List<DailyReminderDto>();
        public List<QuarterlyReminderDto> QuarterlyReminders { get; set; } = new List<QuarterlyReminderDto>();
        public List<HalfYearlyReminderDto> HalfYearlyReminders { get; set; } = new List<HalfYearlyReminderDto>();
    }
}
