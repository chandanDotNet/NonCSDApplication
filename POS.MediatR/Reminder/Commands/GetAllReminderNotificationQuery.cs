using POS.Data.Entities;
using POS.Data.Resources;
using POS.Helper;
using MediatR;

namespace POS.MediatR.CommandAndQuery
{
    public class GetAllReminderNotificationQuery : IRequest<PagedList<ReminderScheduler>>
    {
        public ReminderResource ReminderResource { get; set; }
    }
}
