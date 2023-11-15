using POS.Data.Entities;
using System;

namespace POS.Data.Dto
{
    public class ReminderSchedulerDto
    {
        public Guid Id { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public bool IsRead { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime Duration { get; set; }
        public Guid? ReferenceId { get; set; }
        public ApplicationEnums? Application { get; set; }
        public string UserName { get; set; }
    }
}
