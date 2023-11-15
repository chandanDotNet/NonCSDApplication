using AutoMapper;
using POS.Common.GenericRepository;
using POS.Common.UnitOfWork;
using POS.Data;
using POS.Data.Dto;
using POS.Data.Resources;
using POS.Domain;
using POS.Helper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace POS.Repository
{
    public class InquiryRepository : GenericRepository<Inquiry, POSDbContext>, IInquiryRepository
    {
        private readonly IPropertyMappingService _propertyMappingService;
        private readonly IMapper _mapper;
        public InquiryRepository(
            IUnitOfWork<POSDbContext> uow,
            IPropertyMappingService propertyMappingService,
             IMapper mapper)
            : base(uow)
        {
            _mapper = mapper;
            _propertyMappingService = propertyMappingService;
        }

        public async Task<InquiryList> GetInquiries(InquiryResource inquiryResource)
        {
            var collectionBeforePaging =
                All.ApplySort(inquiryResource.OrderBy,
                _propertyMappingService.GetPropertyMapping<InquiryDto, Inquiry>());
            if (!string.IsNullOrEmpty(inquiryResource.CompanyName))
            {
                // trim & ignore casing
                var genreForWhereClause = inquiryResource.CompanyName
                    .Trim().ToLowerInvariant();
                var name = Uri.UnescapeDataString(genreForWhereClause);
                var encodingName = WebUtility.UrlDecode(name);
                var ecapestring = Regex.Unescape(encodingName);
                encodingName = encodingName.Replace(@"\", @"\\").Replace("%", @"\%").Replace("_", @"\_").Replace("[", @"\[").Replace(" ", "%");
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => EF.Functions.Like(a.CompanyName, $"{encodingName}%"));
            }
            if (!string.IsNullOrEmpty(inquiryResource.ContactPerson))
            {
                // trim & ignore casing
                var genreForWhereClause = inquiryResource.ContactPerson
                    .Trim().ToLowerInvariant();
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => EF.Functions.Like(a.ContactPerson, $"{genreForWhereClause}%"));
            }
            if (!string.IsNullOrEmpty(inquiryResource.PhoneNo))
            {
                // trim & ignore casing
                var searchQueryForWhereClause = inquiryResource.PhoneNo
                    .Trim().ToLowerInvariant();
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.Phone != null && EF.Functions.Like(a.Phone, $"{searchQueryForWhereClause}%"));
            }
            if (!string.IsNullOrEmpty(inquiryResource.MobileNo))
            {
                // trim & ignore casing
                var searchQueryForWhereClause = inquiryResource.MobileNo
                    .Trim().ToLowerInvariant();
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.MobileNo != null && EF.Functions.Like(a.MobileNo, $"{searchQueryForWhereClause}%"));
            }
            if (!string.IsNullOrEmpty(inquiryResource.Email))
            {
                // trim & ignore casing
                var searchQueryForWhereClause = inquiryResource.Email
                    .Trim().ToLowerInvariant();
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.Email != null && EF.Functions.Like(a.Email, $"{searchQueryForWhereClause}%"));
            }
            if (!string.IsNullOrEmpty(inquiryResource.CityName))
            {
                // trim & ignore casing
                var searchQueryForWhereClause = inquiryResource.CityName
                    .Trim().ToLowerInvariant();
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.CityName != null && EF.Functions.Like(a.CityName, $"{searchQueryForWhereClause}%"));
            }
            if (!string.IsNullOrEmpty(inquiryResource.CountryName))
            {
                // trim & ignore casing
                var searchQueryForWhereClause = inquiryResource.CountryName
                    .Trim().ToLowerInvariant();
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.CountryName != null && EF.Functions.Like(a.CountryName, $"{searchQueryForWhereClause}%"));
            }
            if (inquiryResource.InquirySourceId.HasValue)
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.InquirySourceId == inquiryResource.InquirySourceId);
            }
            if (!string.IsNullOrEmpty(Convert.ToString(inquiryResource.AssignTo)))
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.AssignTo == inquiryResource.AssignTo);
            }
            if (!string.IsNullOrEmpty(Convert.ToString(inquiryResource.InquiryStatusId)))
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.InquiryStatusId == inquiryResource.InquiryStatusId);
            }

            if (!string.IsNullOrEmpty(inquiryResource.SearchQuery))
            {
                // trim & ignore casing
                var searchQueryForWhereClause = inquiryResource.SearchQuery
                    .Trim().ToLowerInvariant();
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => (a.Email != null && EF.Functions.Like(a.Email, $"{searchQueryForWhereClause}%"))
                    || EF.Functions.Like(a.CompanyName, $"%{searchQueryForWhereClause}%")
                    || (a.MobileNo != null && EF.Functions.Like(a.MobileNo, $"{searchQueryForWhereClause}%"))
                    || (a.Phone != null && EF.Functions.Like(a.Phone, $"{searchQueryForWhereClause}%"))
                    || EF.Functions.Like(a.ContactPerson, $"{searchQueryForWhereClause}%")
                    );
            }

            return await new InquiryList().Create(collectionBeforePaging,
                inquiryResource.Skip,
                inquiryResource.PageSize);
        }
    }
}
