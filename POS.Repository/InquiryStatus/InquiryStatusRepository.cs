using AutoMapper;
using POS.Common.GenericRepository;
using POS.Common.UnitOfWork;
using POS.Data.Entities;
using POS.Domain;

namespace POS.Repository
{
    public class InquiryStatusRepository : GenericRepository<InquiryStatus, POSDbContext>, IInquiryStatusRepository
    {
        private readonly IPropertyMappingService _propertyMappingService;
        private readonly IMapper _mapper;
        public InquiryStatusRepository(
            IUnitOfWork<POSDbContext> uow,
            IPropertyMappingService propertyMappingService,
             IMapper mapper)
            : base(uow)
        {
            _mapper = mapper;
            _propertyMappingService = propertyMappingService;
        }
    }
}
