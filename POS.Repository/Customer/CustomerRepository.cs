using AutoMapper;
using Microsoft.EntityFrameworkCore;
using POS.Common.GenericRepository;
using POS.Common.UnitOfWork;
using POS.Data;
using POS.Data.Dto;
using POS.Domain;
using System;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace POS.Repository
{
    public class CustomerRepository : GenericRepository<Customer, POSDbContext>, ICustomerRepository
    {
        private readonly IPropertyMappingService _propertyMappingService;
        private readonly IMapper _mapper;
        private readonly ISalesOrderRepository _salesOrderRepository;

        public CustomerRepository(IUnitOfWork<POSDbContext> uow,
            IPropertyMappingService propertyMappingService,
            IMapper mapper,
            ISalesOrderRepository salesOrderRepository)
            : base(uow)
        {
            _propertyMappingService = propertyMappingService;
            _mapper = mapper;
            _salesOrderRepository = salesOrderRepository;
        }

        public async Task<CustomerList> GetCustomers(CustomerResource customerResource)
        {
            var collectionBeforePaging =
                All.ApplySort(customerResource.OrderBy,
                _propertyMappingService.GetPropertyMapping<CustomerDto, Customer>());

            if (!string.IsNullOrEmpty(customerResource.CustomerName))
            {
                // trim & ignore casing
                var genreForWhereClause = customerResource.CustomerName
                    .Trim().ToLowerInvariant();
                var name = Uri.UnescapeDataString(genreForWhereClause);
                var encodingName = WebUtility.UrlDecode(name);
                var ecapestring = Regex.Unescape(encodingName);
                encodingName = encodingName.Replace(@"\", @"\\").Replace("%", @"\%").Replace("_", @"\_").Replace("[", @"\[").Replace(" ", "%");
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => EF.Functions.Like(a.CustomerName, $"{encodingName}%"));
            }

            //if (!string.IsNullOrEmpty(customerResource.CustomerName))
            //{
            //    // trim & ignore casing
            //    var genreForWhereClause = customerResource.CustomerName
            //        .Trim().ToLowerInvariant();
            //    collectionBeforePaging = collectionBeforePaging
            //        .Where(a => EF.Functions.Like(a.CustomerName, $"{genreForWhereClause}%"));
            //}
            if (!string.IsNullOrEmpty(customerResource.ContactPerson))
            {
                // trim & ignore casing
                var genreForWhereClause = customerResource.ContactPerson
                    .Trim().ToLowerInvariant();
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => EF.Functions.Like(a.ContactPerson, $"{genreForWhereClause}%"));
            }
            if (!string.IsNullOrEmpty(customerResource.PhoneNo))
            {
                // trim & ignore casing
                var searchQueryForWhereClause = customerResource.PhoneNo
                    .Trim().ToLowerInvariant();
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.PhoneNo != null && EF.Functions.Like(a.PhoneNo, $"{searchQueryForWhereClause}%"));
            }
            if (!string.IsNullOrEmpty(customerResource.MobileNo))
            {
                // trim & ignore casing
                var searchQueryForWhereClause = customerResource.MobileNo
                    .Trim().ToLowerInvariant();
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.MobileNo != null && EF.Functions.Like(a.MobileNo, $"{searchQueryForWhereClause}%"));
            }
            if (!string.IsNullOrEmpty(customerResource.Email))
            {
                // trim & ignore casing
                var searchQueryForWhereClause = customerResource.Email
                    .Trim().ToLowerInvariant();
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.Email != null && EF.Functions.Like(a.Email, $"{searchQueryForWhereClause}%"));
            }
            if (!string.IsNullOrEmpty(customerResource.Website))
            {
                // trim & ignore casing
                var searchQueryForWhereClause = customerResource.Website
                    .Trim().ToLowerInvariant();
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.Website != null && EF.Functions.Like(a.Website, $"{searchQueryForWhereClause}%"));
            }

            if (!string.IsNullOrEmpty(customerResource.SearchQuery))
            {
                // trim & ignore casing
                var searchQueryForWhereClause = customerResource.SearchQuery
                    .Trim().ToLowerInvariant();
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => (a.Email != null && EF.Functions.Like(a.Email, $"{searchQueryForWhereClause}%"))
                    || EF.Functions.Like(a.CustomerName, $"%{searchQueryForWhereClause}%")
                    || EF.Functions.Like(a.MobileNo, $"{searchQueryForWhereClause}%")
                    || (a.PhoneNo != null && EF.Functions.Like(a.PhoneNo, $"{searchQueryForWhereClause}%"))
                    || EF.Functions.Like(a.PhoneNo, $"{searchQueryForWhereClause}%")
                    );
            }

            var CustomerList = new CustomerList(_mapper);
            return await CustomerList.Create(collectionBeforePaging,
                customerResource.Skip,
                customerResource.PageSize);
        }

        public async Task<CustomerPaymentList> GetCustomersPayment(CustomerResource customerResource)
        {
            var collectionBeforePaging =
                _salesOrderRepository
                .AllIncluding(c => c.Customer)
                .ApplySort(customerResource.OrderBy,
                _propertyMappingService.GetPropertyMapping<SalesOrderDto, SalesOrder>());

            if (!string.IsNullOrEmpty(customerResource.CustomerName))
            {
                // trim & ignore casing
                var genreForWhereClause = customerResource.CustomerName
                    .Trim().ToLowerInvariant();
                var name = Uri.UnescapeDataString(genreForWhereClause);
                var encodingName = WebUtility.UrlDecode(name);
                var ecapestring = Regex.Unescape(encodingName);
                encodingName = encodingName.Replace(@"\", @"\\").Replace("%", @"\%").Replace("_", @"\_").Replace("[", @"\[").Replace(" ", "%");
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => EF.Functions.Like(a.Customer.CustomerName, $"{encodingName}%"));
            }

            var groupedCollection = collectionBeforePaging.GroupBy(c => c.CustomerId);

            var supplierPayments = new CustomerPaymentList();
            return await supplierPayments.Create(groupedCollection, customerResource.Skip, customerResource.PageSize);
        }

        public async Task<CustomerPaymentList> GetCustomersPaymentReport(CustomerResource customerResource)
        {
            var collectionBeforePaging =
                _salesOrderRepository
                .AllIncluding(c => c.Customer)
                .ApplySort(customerResource.OrderBy,
                _propertyMappingService.GetPropertyMapping<SalesOrderDto, SalesOrder>());

            if (!string.IsNullOrEmpty(customerResource.CustomerName))
            {
                // trim & ignore casing
                var genreForWhereClause = customerResource.CustomerName
                    .Trim().ToLowerInvariant();
                var name = Uri.UnescapeDataString(genreForWhereClause);
                var encodingName = WebUtility.UrlDecode(name);
                var ecapestring = Regex.Unescape(encodingName);
                encodingName = encodingName.Replace(@"\", @"\\").Replace("%", @"\%").Replace("_", @"\_").Replace("[", @"\[").Replace(" ", "%");
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => EF.Functions.Like(a.Customer.CustomerName, $"{encodingName}%"));
            }

            var groupedCollection = collectionBeforePaging.GroupBy(c => c.CustomerId);

            var supplierPayments = new CustomerPaymentList();
            return await supplierPayments.Create(groupedCollection, 0, 0);
        }


        public async Task<CustomerList> GetCustomersData(CustomerResource customerResource)
        {
            var collectionBeforePaging =
                All.ApplySort(customerResource.OrderBy,
                _propertyMappingService.GetPropertyMapping<CustomerDto, Customer>());           

            
            if (!string.IsNullOrEmpty(customerResource.MobileNo))
            {
                // trim & ignore casing
                var searchQueryForWhereClause = customerResource.MobileNo
                    .Trim().ToLowerInvariant();
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.MobileNo == searchQueryForWhereClause);
            }           

            var CustomerList = new CustomerList(_mapper);
            return await CustomerList.Create(collectionBeforePaging,
                customerResource.Skip,
                customerResource.PageSize);
        }
    }
}
