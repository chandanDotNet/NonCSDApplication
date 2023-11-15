using POS.Data.Resources;
using POS.Repository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.MediatR.CommandAndQuery
{
    public class GetAllReminderQuery : IRequest<ReminderList>
    {
        public ReminderResource ReminderResource { get; set; }
    }
}
