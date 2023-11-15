using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using POS.API.Helpers;
using POS.Data;
using POS.Data.Dto;
using POS.Data.Resources;
using POS.MediatR.Inventory.Command;
using POS.MediatR.InventoryHistory.Command;
using POS.Repository;
using System;
using System.Threading.Tasks;

namespace POS.API.Controllers.Inventory
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class InventoryController : BaseController
    {
        private readonly IMediator _mediator;

        public InventoryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get All Inventory.
        /// </summary>
        /// <param name="inventoryResource"></param>
        /// <returns></returns>
        [HttpGet]
        [ClaimCheck("INVE_VIEW_INVENTORIES")]
        [Produces("application/json", "application/xml", Type = typeof(InventoryList))]
        public async Task<IActionResult> GetInventories([FromQuery] InventoryResource inventoryResource)
        {
            var getAllInventoryCommand = new GetAllInventoryCommand
            {
                InventoryResource = inventoryResource
            };
            var result = await _mediator.Send(getAllInventoryCommand);

            var paginationMetadata = new
            {
                totalCount = result.TotalCount,
                pageSize = result.PageSize,
                skip = result.Skip,
                totalPages = result.TotalPages
            };
            Response.Headers.Add("X-Pagination",
                Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetadata));
            return Ok(result);
        }

        /// <summary>
        /// Add Inventory
        /// </summary>
        /// <param name="addInventoryCommand"></param>
        /// <returns></returns>
        [HttpPost]
        [ClaimCheck("INVE_MANAGE_INVENTORY")]
        [Produces("application/json", "application/xml", Type = typeof(InventoryDto))]
        public async Task<IActionResult> AddInventory(AddInventoryCommand addInventoryCommand)
        {
            var result = await _mediator.Send(addInventoryCommand);
            return ReturnFormattedResponse(result);
        }

        /// <summary>
        /// Get Inventory History.
        /// </summary>
        /// <param name="inventoryHistoryResource"></param>
        /// <returns></returns>
        [HttpGet("history")]
        [ClaimCheck("INVE_VIEW_INVENTORIES")]
        [Produces("application/json", "application/xml", Type = typeof(InventoryHistoryList))]
        public async Task<IActionResult> GetInventoryHistory([FromQuery] InventoryHistoryResource inventoryHistoryResource)
        {
            var getAllInventoryCommand = new GetAllInventoryHistoryByProductCommand
            {
                InventoryHistoryResource = inventoryHistoryResource
            };

            var result = await _mediator.Send(getAllInventoryCommand);

            var paginationMetadata = new
            {
                totalCount = result.TotalCount,
                pageSize = result.PageSize,
                skip = result.Skip,
                totalPages = result.TotalPages
            };
            Response.Headers.Add("X-Pagination",
                Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetadata));
            return Ok(result);
        }
    }
}
