using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Data
{
    public class ReminderUser
    {
        public Guid ReminderId { get; set; }
        public Guid UserId { get; set; }
        public Reminder Reminder { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
    }
}
