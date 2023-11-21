using AutoMapper;
using Microsoft.EntityFrameworkCore;
using POS.Common.GenericRepository;
using POS.Common.UnitOfWork;
using POS.Data;
using POS.Data.Dto;
using POS.Data.Resources;
using POS.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace POS.Repository
{
    public class CustomerAddressRepository : GenericRepository<CustomerAddress, POSDbContext>, ICustomerAddressRepository
    {
        private readonly IPropertyMappingService _propertyMappingService;
        public CustomerAddressRepository(
            IUnitOfWork<POSDbContext> uow,
            IPropertyMappingService propertyMappingService,
             IMapper mapper)
            : base(uow)
        {
            _propertyMappingService = propertyMappingService;
        }

        public async Task<CustomerAddressList> GetCustomerAddresses(CustomerAddressResource customerAddressResource)
        {
            var collectionBeforePaging =
                AllIncluding(c => c.Customer).ApplySort(customerAddressResource.OrderBy,
                _propertyMappingService.GetPropertyMapping<CustomerAddressDto, CustomerAddress>());            

            if (!string.IsNullOrEmpty(customerAddressResource.CustomerId.ToString()))
            {
                // trim & ignore casing
                var genreForWhereClause = customerAddressResource.CustomerId.ToString();
                var name = Uri.UnescapeDataString(genreForWhereClause);
                var encodingName = WebUtility.UrlDecode(name);
                var ecapestring = Regex.Unescape(encodingName);
                encodingName = encodingName.Replace(@"\", @"\\").Replace("%", @"\%").Replace("_", @"\_").Replace("[", @"\[").Replace(" ", "%");
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => EF.Functions.Like(a.CustomerId.ToString(), $"{encodingName}%"));
            }

            if (!string.IsNullOrWhiteSpace(customerAddressResource.CustomerName))
            {
                collectionBeforePaging = collectionBeforePaging
                  .Where(a => a.Customer.CustomerName == customerAddressResource.CustomerName);
            }

            var CustomerAddressList = new CustomerAddressList();
            return await CustomerAddressList.Create(collectionBeforePaging, customerAddressResource.Skip, customerAddressResource.PageSize);
        }
    }
}