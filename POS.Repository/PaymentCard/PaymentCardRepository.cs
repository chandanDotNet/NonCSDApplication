using AutoMapper;
using Microsoft.EntityFrameworkCore;
using POS.Common.GenericRepository;
using POS.Common.UnitOfWork;
using POS.Data.Dto;
using POS.Data.Resources;
using POS.Data;
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
    public class PaymentCardRepository : GenericRepository<PaymentCard, POSDbContext>, IPaymentCardRepository
    {
        private readonly IPropertyMappingService _propertyMappingService;
        public PaymentCardRepository(
            IUnitOfWork<POSDbContext> uow,
            IPropertyMappingService propertyMappingService,
             IMapper mapper)
            : base(uow)
        {
            _propertyMappingService = propertyMappingService;
        }

        public async Task<PaymentCardList> GetPaymentCards(PaymentCardResource paymentCardResource)
        {
            var collectionBeforePaging =
                AllIncluding(c => c.Customer).ApplySort(paymentCardResource.OrderBy,
                _propertyMappingService.GetPropertyMapping<PaymentCardDto, PaymentCard>());

            if (!string.IsNullOrEmpty(paymentCardResource.CustomerId.ToString()))
            {
                // trim & ignore casing
                var genreForWhereClause = paymentCardResource.CustomerId.ToString();
                var name = Uri.UnescapeDataString(genreForWhereClause);
                var encodingName = WebUtility.UrlDecode(name);
                var ecapestring = Regex.Unescape(encodingName);
                encodingName = encodingName.Replace(@"\", @"\\").Replace("%", @"\%").Replace("_", @"\_").Replace("[", @"\[").Replace(" ", "%");
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => EF.Functions.Like(a.CustomerId.ToString(), $"{encodingName}%"));
            }

            if (!string.IsNullOrWhiteSpace(paymentCardResource.CustomerName))
            {
                collectionBeforePaging = collectionBeforePaging
                  .Where(a => a.Customer.CustomerName == paymentCardResource.CustomerName);
            }

            var PaymentCardList = new PaymentCardList();
            return await PaymentCardList.Create(collectionBeforePaging, paymentCardResource.Skip, paymentCardResource.PageSize);
        }
    }
}