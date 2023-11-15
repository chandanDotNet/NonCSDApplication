using AutoMapper;
using POS.Common.GenericRepository;
using POS.Common.UnitOfWork;
using POS.Data;
using POS.Data.Dto;
using POS.Data.Resources;
using POS.Domain;
using POS.Helper;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace POS.Repository
{
    public class ContactUsRepository : GenericRepository<ContactRequest, POSDbContext>, IContactUsRepository
    {
        private readonly IPropertyMappingService _propertyMappingService;
        private readonly IMapper _mapper;

        public ContactUsRepository(IUnitOfWork<POSDbContext> uow,
            IPropertyMappingService propertyMappingService,
            IMapper mapper)
            : base(uow)
        {
            _propertyMappingService = propertyMappingService;
            _mapper = mapper;
        }

        public async Task<PagedList<ContactRequest>> GetContactUsList(ContactUsResource contactUsResource)
        {
            var collectionBeforePaging =
                All.ApplySort(contactUsResource.OrderBy,
                _propertyMappingService.GetPropertyMapping<ContactUsDto, ContactRequest>());
            if (!string.IsNullOrEmpty(contactUsResource.Name))
            {
                // trim & ignore casing
                var genreForWhereClause = contactUsResource.Name
                    .Trim().ToLowerInvariant();
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => EF.Functions.Like(a.Name, $"{genreForWhereClause}%"));
            }
            if (!string.IsNullOrEmpty(contactUsResource.Email))
            {
                // trim & ignore casing
                var genreForWhereClause = contactUsResource.Email
                    .Trim().ToLowerInvariant();
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => EF.Functions.Like(a.Email, $"{genreForWhereClause}%"));
            }
            if (!string.IsNullOrEmpty(contactUsResource.Phone))
            {
                // trim & ignore casing
                var searchQueryForWhereClause = contactUsResource.Phone
                    .Trim().ToLowerInvariant();
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.Phone != null && EF.Functions.Like(a.Phone, $"{searchQueryForWhereClause}%"));
            }
        

            return await PagedList<ContactRequest>.Create(collectionBeforePaging,
                contactUsResource.Skip,
                contactUsResource.PageSize);
        }
    }
}
