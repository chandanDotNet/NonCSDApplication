using AutoMapper;
using POS.Common.GenericRepository;
using POS.Common.UnitOfWork;
using POS.Data;
using POS.Data.Dto;
using POS.Data.Resources;
using POS.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Repository
{
    public class WishlistRepository : GenericRepository<Wishlist, POSDbContext>, IWishlistRepository
    {

        private readonly IPropertyMappingService _propertyMappingService;
        private readonly IMapper _mapper;
        

        public WishlistRepository(IUnitOfWork<POSDbContext> uow,
            IPropertyMappingService propertyMappingService,
            IMapper mapper,
            ISalesOrderRepository salesOrderRepository)
            : base(uow)
        {
            _propertyMappingService = propertyMappingService;
            _mapper = mapper;            
        }

        public async Task<AllWishList> GetWishlistData(WishlistResource cartResource)
        {
            //var collectionBeforePaging =
            //    All.ApplySort(cartResource.OrderBy,
            //    _propertyMappingService.GetPropertyMapping<WishlistDto, Wishlist>()).Where(a => a.CustomerId == cartResource.CustomerId);

            var collectionBeforePaging =
               AllIncluding(c => c.Product).ApplySort(cartResource.OrderBy,
               _propertyMappingService.GetPropertyMapping<WishlistDto, Wishlist>()).Where(a => a.CustomerId == cartResource.CustomerId);

            //var collectionBeforePaging1 =
            //    All.ApplySort(cartResource.OrderBy,
            //    _propertyMappingService.GetPropertyMapping<ProductDto, Product>()).Where(a => a.ProductId == cartResource.ProductId);

            var AllWishList = new AllWishList(_mapper);
            return await AllWishList.Create(collectionBeforePaging,
                cartResource.Skip,
                cartResource.PageSize);
        }

    }
}
