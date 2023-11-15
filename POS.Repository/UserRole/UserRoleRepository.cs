using POS.Common.GenericRepository;
using POS.Common.UnitOfWork;
using POS.Data;
using POS.Domain;

namespace POS.Repository
{
    public class UserRoleRepository : GenericRepository<UserRole, POSDbContext>,
       IUserRoleRepository
    {
        public UserRoleRepository(
            IUnitOfWork<POSDbContext> uow
            ) : base(uow)
        {
        }
    }
}
