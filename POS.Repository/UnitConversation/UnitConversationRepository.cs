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
    public class UnitConversationRepository : GenericRepository<UnitConversation, POSDbContext>, IUnitConversationRepository
    {
        public UnitConversationRepository(IUnitOfWork<POSDbContext> uow)
          : base(uow)
        {
        }
    }
}
