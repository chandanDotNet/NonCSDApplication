using POS.Common.GenericRepository;
using POS.Common.UnitOfWork;
using POS.Data;
using POS.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Repository
{
    public class ReminderUserRepository : GenericRepository<ReminderUser, POSDbContext>,
        IReminderUserRepository
    {
        public ReminderUserRepository(
            IUnitOfWork<POSDbContext> uow
            ) : base(uow)
        {
        }
    }
}
