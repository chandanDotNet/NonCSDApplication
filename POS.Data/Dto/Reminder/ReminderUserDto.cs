using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Data.Dto
{
    public class ReminderUserDto
    {
        public Guid? ReminderId { get; set; }
        public Guid UserId { get; set; }
    }
}
