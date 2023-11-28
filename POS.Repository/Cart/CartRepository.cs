using POS.Common.GenericRepository;
using POS.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POS.Common.UnitOfWork;
using POS.Data;
using AutoMapper;
using POS.Data.Dto;
using Microsoft.IdentityModel.Tokens;
using POS.Data.Resources;

namespace POS.Repository
{
    public class CartRepository : GenericRepository<Cart, POSDbContext>, ICartRepository
    {

        private readonly IPropertyMappingService _propertyMappingService;
        private readonly IMapper _mapper;
        private readonly ISalesOrderRepository _salesOrderRepository;

        public CartRepository(IUnitOfWork<POSDbContext> uow,
            IPropertyMappingService propertyMappingService,
            IMapper mapper,
            ISalesOrderRepository salesOrderRepository)
            : base(uow)
        {
            _propertyMappingService = propertyMappingService;
            _mapper = mapper;
            _salesOrderRepository = salesOrderRepository;
        }

        public async Task<CartList> GetCartsData(CartResource cartResource)
        {
            var collectionBeforePaging =
                AllIncluding(c => c.Product).ApplySort(cartResource.OrderBy,
                _propertyMappingService.GetPropertyMapping<CartDto, Cart>()).Where(a => a.CustomerId == cartResource.CustomerId);


            //if (cartResource.CustomerId!= Guid.Empty)
            //{
            //    collectionBeforePaging = collectionBeforePaging
            //        .Where(a => a.CustomerId == cartResource.CustomerId);
            //}

            var CartList = new CartList(_mapper);
            return await CartList.Create(collectionBeforePaging,
                cartResource.Skip,
                cartResource.PageSize);
        }
    }
}
