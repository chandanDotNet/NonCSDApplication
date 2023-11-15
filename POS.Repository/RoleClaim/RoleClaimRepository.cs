using POS.Common.GenericRepository;
using POS.Common.UnitOfWork;
using POS.Data;
using POS.Domain;

namespace POS.Repository
{
    public class RoleClaimRepository : GenericRepository<RoleClaim, POSDbContext>,
           IRoleClaimRepository
    {
        public RoleClaimRepository(
            IUnitOfWork<POSDbContext> uow
            ) : base(uow)
        {
        }
    }
}