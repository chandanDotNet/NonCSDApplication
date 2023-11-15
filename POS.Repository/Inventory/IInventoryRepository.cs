using AutoMapper;
using Microsoft.EntityFrameworkCore;
using POS.Common.GenericRepository;
using POS.Common.UnitOfWork;
using POS.Data;
using POS.Data.Dto;
using POS.Data.Resources;
using POS.Domain;
using POS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace POS.Repository
{
    public interface IInventoryRepository : IGenericRepository<Inventory>
    {
        Task<InventoryList> GetInventories(InventoryResource inventoryResource);
        Task AddInventory(InventoryDto inventory);
        Task AddWarehouseInventory(InventoryDto inventory);
        InventoryDto ConvertStockAndPriceToBaseUnit(InventoryDto inventory);
        Task RemoveExistingWareHouseInventory(List<InventoryDto> inventories);
    }
}
