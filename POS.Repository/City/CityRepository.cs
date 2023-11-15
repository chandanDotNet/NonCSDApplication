using AutoMapper;
using Microsoft.EntityFrameworkCore;
using POS.Common.GenericRepository;
using POS.Common.UnitOfWork;
using POS.Data;
using POS.Data.Dto;
using POS.Data.Resources;
using POS.Domain;
using System;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace POS.Repository
{
    public class CityRepository : GenericRepository<City, POSDbContext>, ICityRepository
    {
        private readonly IPropertyMappingService _propertyMappingService;

        public CityRepository(
            IUnitOfWork<POSDbContext> uow,
            IPropertyMappingService propertyMappingService,
             IMapper mapper)
            : base(uow)
        {
            _propertyMappingService = propertyMappingService;
        }

        public async Task<CityList> GetCities(CityResource cityResource)
        {
            var collectionBeforePaging =
                AllIncluding(c => c.Country).ApplySort(cityResource.OrderBy,
                _propertyMappingService.GetPropertyMapping<CityDto, City>());

            if (!string.IsNullOrEmpty(cityResource.CityName))
            {
                // trim & ignore casing
                var genreForWhereClause = cityResource.CityName
                    .Trim().ToLowerInvariant();
                var name = Uri.UnescapeDataString(genreForWhereClause);
                var encodingName = WebUtility.UrlDecode(name);
                var ecapestring = Regex.Unescape(encodingName);
                encodingName = encodingName.Replace(@"\", @"\\").Replace("%", @"\%").Replace("_", @"\_").Replace("[", @"\[").Replace(" ", "%");
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => EF.Functions.Like(a.CityName, $"{encodingName}%"));
            }

            if (!string.IsNullOrWhiteSpace(cityResource.CountryName))
            {
                collectionBeforePaging = collectionBeforePaging
                  .Where(a => a.Country.CountryName == cityResource.CountryName);
            }

            var CityList = new CityList();
            return await CityList.Create(collectionBeforePaging, cityResource.Skip, cityResource.PageSize);
        }
    }
}