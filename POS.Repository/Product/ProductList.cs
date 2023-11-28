using AutoMapper;
using POS.Data;
using POS.Data.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using POS.Helper;
using System.IO;

namespace POS.Repository
{
    public class ProductList : List<ProductDto>
    {
        public IMapper _mapper { get; set; }
        public PathHelper _pathHelper { get; set; }
        public ProductList(IMapper mapper, PathHelper pathHelper)
        {
            _mapper = mapper;
            _pathHelper = pathHelper;
        }

        public int Skip { get; private set; }
        public int TotalPages { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }

        public ProductList(List<ProductDto> items, int count, int skip, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            Skip = skip;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            AddRange(items);
        }

        public async Task<ProductList> Create(IQueryable<Product> source, int skip, int pageSize)
        {
            var count = await GetCount(source);
            var dtoList = await GetDtos(source, skip, pageSize);
            var dtoPageList = new ProductList(dtoList, count, skip, pageSize);
            return dtoPageList;
        }

        public async Task<int> GetCount(IQueryable<Product> source)
        {
            return await source.AsNoTracking().CountAsync();
        }

        public async Task<List<ProductDto>> GetDtos(IQueryable<Product> source, int skip, int pageSize)
        {
            var entities = await source
                .Skip(skip)
                .Take(pageSize)
                .AsNoTracking()
                .Select(c => new ProductDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Code = c.Code,
                    SkuName = c.SkuName,
                    SkuCode = c.SkuCode,
                    QRCodeUrl = c.QRCodeUrl,
                    Description= c.Description,
                    CreatedDate = c.CreatedDate,                   
                    Mrp = c.Mrp,
                    SalesPrice = c.SalesPrice,
                    PurchasePrice = c.PurchasePrice,
                    CategoryId = c.CategoryId,
                    CategoryName = c.ProductCategory.Name,
                    UnitName = c.Unit.Name,
                    UnitId = c.UnitId,
                    BrandId = c.BrandId,
                    Barcode = c.Barcode,
                    BrandName = c.Brand.Name,
                    WarehouseId = c.WarehouseId,
                    WarehouseName = c.Warehouse.Name, 
                    Discount=0,
                    ProductTaxes = _mapper.Map<List<ProductTaxDto>>(c.ProductTaxes),
                    Unit=_mapper.Map<UnitConversationDto>(c.Unit),
                    ProductUrl = !string.IsNullOrWhiteSpace(c.ProductUrl) ? Path.Combine(_pathHelper.ProductThumbnailImagePath, c.ProductUrl) : "",
                    // Cart= _mapper.Map<CartDto>(c.Cart),
                   

                }).ToListAsync();
            return entities;
        }
    }
}
