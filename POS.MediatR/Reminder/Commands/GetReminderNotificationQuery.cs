using MediatR;
using POS.Data.Resources;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.MediatR.Reminder.Commands
{
    public class GetReminderNotificationQuery : IRequest<ReminderList>
    {
        public ReminderResource ReminderResource { get; set; }
    }
}

