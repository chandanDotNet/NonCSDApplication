using POS.Data.Dto;
using POS.MediatR.CommandAndQuery;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using POS.MediatR.Warehouse.Commands;

namespace POS.API.Controllers.Warehouse

{
    [Route("api")]
    [ApiController]
    [Authorize]
    public class WarehouseController : BaseController
    {
        public IMediator _mediator { get; set; }

        public WarehouseController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get Warehouse.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("Warehouse/{id}", Name = "GetWarehouse")]
        [Produces("application/json", "application/xml", Type = typeof(WarehouseDto))]
        public async Task<IActionResult> GetWarehouse(Guid id)
        {
            var getWarehouseCommand = new GetWarehouseCommand { Id = id };
            var result = await _mediator.Send(getWarehouseCommand);
            return ReturnFormattedResponse(result);
        }

        /// <summary>
        /// Get Warehouse.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("Warehouse/product/{id}", Name = "GetWarehouseByProductId")]
        [Produces("application/json", "application/xml", Type = typeof(WarehouseDto))]
        public async Task<IActionResult> GetWarehousesByProductId(Guid id)
        {
            var getWarehousesByProductIdCommand = new GetWarehousesByProductIdCommand { ProductId = id };
            var result = await _mediator.Send(getWarehousesByProductIdCommand);
            return ReturnFormattedResponse(result);
        }

        /// <summary>
        /// Get Warehouses.
        /// </summary>
        /// <returns></returns>
        [HttpGet("Warehouses")]
        [Produces("application/json", "application/xml", Type = typeof(List<WarehouseDto>))]
        public async Task<IActionResult> GetWarehouses()
        {
            var getAllWarehouseCommand = new GetAllWarehouseCommand { };
            var result = await _mediator.Send(getAllWarehouseCommand);
            return Ok(result);
        }

        /// <summary>
        /// Create Warehouse.
        /// </summary>
        /// <param name="addWarehouseCommand"></param>
        /// <returns></returns>
        [HttpPost("Warehouse")]
        [Produces("application/json", "application/xml", Type = typeof(WarehouseDto))]
        public async Task<IActionResult> AddWarehouse(AddWarehouseCommand addWarehouseCommand)
        {
            var response = await _mediator.Send(addWarehouseCommand);
            if (!response.Success)
            {
                return ReturnFormattedResponse(response);
            }
            return CreatedAtAction("GetWarehouse", new { id = response.Data.Id }, response.Data);
        }

        /// <summary>
        /// Update Warehouse.
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="updateWarehouseCommand"></param>
        /// <returns></returns>
        [HttpPut("Warehouse/{Id}")]
        [Produces("application/json", "application/xml", Type = typeof(WarehouseDto))]
        public async Task<IActionResult> UpdateWarehouse(Guid Id, UpdateWarehouseCommand updateWarehouseCommand)
        {
            updateWarehouseCommand.Id = Id;
            var result = await _mediator.Send(updateWarehouseCommand);
            return ReturnFormattedResponse(result);

        }

        /// <summary>
        /// Delete Warehouse.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpDelete("Warehouse/{Id}")]
        public async Task<IActionResult> DeleteWarehouse(Guid Id)
        {
            var deleteWarehouseCommand = new DeleteWarehouseCommand { Id = Id };
            var result = await _mediator.Send(deleteWarehouseCommand);
            return ReturnFormattedResponse(result);
        }
        /// <summary>
        /// get Warehouse Product.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("Warehouse/{id}/items")]
        [Produces("application/json", "application/xml", Type = typeof(List<WarehouseInventoryDto>))]
        public async Task<IActionResult> GetWarehouseProductItems(Guid id)
        {
            var getWarehouseProductItems = new GetWarehouseProductItemsCommand { Id = id };
            var warehouseProductItems = await _mediator.Send(getWarehouseProductItems);
            return Ok(warehouseProductItems);
        }

    }
}
