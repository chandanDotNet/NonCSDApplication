using POS.Common.GenericRepository;
using POS.Common.UnitOfWork;
using POS.Domain;
using POS.Data;

namespace POS.Repository
{
    public class NewsletterSubscriberRepository : GenericRepository<NewsletterSubscriber, POSDbContext>, INewsletterSubscriberRepository
    {
        public NewsletterSubscriberRepository(IUnitOfWork<POSDbContext> uow)
            : base(uow)
        {
        }
    }
}
