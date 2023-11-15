using POS.Common.GenericRepository;
using POS.Common.UnitOfWork;
using POS.Data;
using POS.Domain;

namespace POS.Repository
{
    public class UserClaimRepository : GenericRepository<UserClaim, POSDbContext>,
           IUserClaimRepository
    {
        public UserClaimRepository(
            IUnitOfWork<POSDbContext> uow
            ) : base(uow)
        {
        }
    }
}