using POS.Data.Dto;
using POS.Data.Resources;
using POS.MediatR.CommandAndQuery;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using POS.MediatR.PurchaseOrder.Commands;
using POS.API.Helpers;
using POS.MediatR.Commands;

namespace POS.API.Controllers.PurchaseOrder
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PurchaseOrderController : BaseController
    {
        public IMediator _mediator { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PurchaseOrderController"/> class.
        /// </summary>
        /// <param name="mediator">The mediator.</param>
        public PurchaseOrderController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Gets all purchase order.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ClaimCheck("POR_VIEW_PO_REQUESTS,PO_VIEW_PURCHASE_ORDERS,REP_PO_REP")]
        [Produces("application/json", "application/xml", Type = typeof(List<PurchaseOrderDto>))]
        public async Task<IActionResult> GetAllPurchaseOrder([FromQuery] PurchaseOrderResource purchaseOrderResource)
        {
            var getAllPurchaseOrderQuery = new GetAllPurchaseOrderQuery
            {
                PurchaseOrderResource = purchaseOrderResource
            };
            var purchaseOrders = await _mediator.Send(getAllPurchaseOrderQuery);

            var paginationMetadata = new
            {
                totalCount = purchaseOrders.TotalCount,
                pageSize = purchaseOrders.PageSize,
                skip = purchaseOrders.Skip,
                totalPages = purchaseOrders.TotalPages
            };

            Response.Headers.Add("X-Pagination",
                Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetadata));

            return Ok(purchaseOrders);
        }

        /// <summary>
        /// Gets all purchase order.
        /// </summary>
        /// <returns></returns>
        [HttpGet("report")]
        [ClaimCheck("POR_VIEW_PO_REQUESTS,PO_VIEW_PURCHASE_ORDERS,REP_PO_REP")]
        [Produces("application/json", "application/xml", Type = typeof(List<PurchaseOrderDto>))]
        public async Task<IActionResult> GetAllPurchaseOrderReport([FromQuery] PurchaseOrderResource purchaseOrderResource)
        {
            var getAllPurchaseOrderQuery = new GetAllPurchaseOrderReportQuery
            {
                PurchaseOrderResource = purchaseOrderResource
            };
            var purchaseOrders = await _mediator.Send(getAllPurchaseOrderQuery);

            var paginationMetadata = new
            {
                totalCount = purchaseOrders.TotalCount,
                pageSize = purchaseOrders.PageSize,
                skip = purchaseOrders.Skip,
                totalPages = purchaseOrders.TotalPages
            };

            Response.Headers.Add("X-Pagination",
                Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetadata));

            return Ok(purchaseOrders);
        }


        [HttpGet("{id}")]
        [ClaimCheck("POR_VIEW_PO_REQUESTS,PO_VIEW_PURCHASE_ORDERS,PO_VIEW_PO_DETAIL")]
        [Produces("application/json", "application/xml", Type = typeof(List<PurchaseOrderDto>))]
        public async Task<IActionResult> GetPurchaseOrder(Guid id)
        {
            var getPurchaseOrderQuery = new GetPurchaseOrderQuery
            {
                Id = id
            };
            var purchaseOrder = await _mediator.Send(getPurchaseOrderQuery);
            return ReturnFormattedResponse(purchaseOrder);
        }





        /// <summary>
        /// Creates the purchase order.
        /// </summary>
        /// <param name="addPurchaseOrderCommand">The add purchase order command.</param>
        /// <returns></returns>
        [HttpPost, DisableRequestSizeLimit]
        [ClaimCheck("PO_ADD_PO,POR_ADD_PO_REQUEST")]
        [Produces("application/json", "application/xml", Type = typeof(PurchaseOrderDto))]
        public async Task<IActionResult> CreatePurchaseOrder(AddPurchaseOrderCommand addPurchaseOrderCommand)
        {
            var result = await _mediator.Send(addPurchaseOrderCommand);
            return ReturnFormattedResponse(result);
        }

        /// <summary>
        /// Update the purchase order.
        /// </summary>
        /// <param name="updatePurchaseOrderCommand">The add purchase order command.</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ClaimCheck("POR_UPDATE_PO_REQUEST,PO_UPDATE_PO")]
        [Produces("application/json", "application/xml", Type = typeof(PurchaseOrderDto))]
        public async Task<IActionResult> UpdatePurchaseOrder(Guid id, UpdatePurchaseOrderCommand updatePurchaseOrderCommand)
        {
            var result = await _mediator.Send(updatePurchaseOrderCommand);
            return ReturnFormattedResponse(result);
        }

        /// <summary>
        /// Update the purchase order Return.
        /// </summary>
        /// <param name="updatePurchaseOrderCommand">The add purchase order command.</param>
        /// <returns></returns>
        [HttpPut("{id}/return")]
        [ClaimCheck("PO_RETURN_PO")]
        [Produces("application/json", "application/xml", Type = typeof(PurchaseOrderDto))]
        public async Task<IActionResult> UpdatePurchaseOrderReturn(Guid id, UpdatePurchaseOrderReturnCommand updatePurchaseOrderCommand)
        {
            var result = await _mediator.Send(updatePurchaseOrderCommand);
            return ReturnFormattedResponse(result);
        }

        /// <summary>
        /// Delete Purchase Order
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ClaimCheck("PO_DELETE_PO,POR_DELETE_PO_REQUEST")]
        public async Task<IActionResult> DeletePurchaseOrder(Guid id)
        {
            var deletePurchaseOrderCommand = new DeletePurchaseOrderCommand
            {
                Id = id
            };
            var response = await _mediator.Send(deletePurchaseOrderCommand);
            return Ok(response);
        }

        /// <summary>
        /// Gets the new purchase order number.
        /// </summary>
        /// <returns></returns>
        [HttpGet("newOrderNumber/{isPurchaseOrder}")]
        public async Task<IActionResult> GetNewPurchaseOrderNumber(bool isPurchaseOrder)
        {
            var getNewPurchaseOrderNumberQuery = new GetNewPurchaseOrderNumberQuery
            {
                isPurchaseOrder = isPurchaseOrder
            };
            var response = await _mediator.Send(getNewPurchaseOrderNumberQuery);
            return Ok(new
            {
                OrderNumber = response
            });
        }

        /// <summary>
        /// Get Purchase Order Items.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isReturn"></param>
        /// <returns></returns>
        [HttpGet("{id}/items")]
        [ClaimCheck("PO_VIEW_PURCHASE_ORDERS")]
        public async Task<IActionResult> GetPurchaseOrderItems(Guid id, bool isReturn = false)
        {
            var getPurchaseOrderItemsCommand = new GetPurchaseOrderItemsCommand { Id = id, IsReturn = isReturn };
            var response = await _mediator.Send(getPurchaseOrderItemsCommand);
            return Ok(response);
        }

        /// <summary>
        /// Get Purchase Order Items.
        /// </summary>
        /// <param name="purchaseOrderResource"></param>
        /// <returns></returns>
        [HttpGet("items/reports")]
        [ClaimCheck("PO_VIEW_PURCHASE_ORDERS")]
        public async Task<IActionResult> GetPurchaseOrderItems([FromQuery] PurchaseOrderResource purchaseOrderResource)
        {
            var getPurchaseOrderItemsCommand = new GetPurchaseOrderItemsReportCommand { PurchaseOrderResource= purchaseOrderResource };
            var response = await _mediator.Send(getPurchaseOrderItemsCommand);

            var paginationMetadata = new
            {
                totalCount = response.TotalCount,
                pageSize = response.PageSize,
                skip = response.Skip,
                totalPages = response.TotalPages
            };

            Response.Headers.Add("X-Pagination",
                Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetadata));

            return Ok(response);

        }

        /// <summary>
        /// Get Recent Expected Date Purchase Order
        /// </summary>
        /// <returns></returns>
        [HttpGet("recent/expecteddate")]
        [ClaimCheck("DB_RECENT_PO_DELIVERY")]
        public async Task<IActionResult> GetRecentExpectedDatePurchaseOrder()
        {
            var getPurchaseOrderRecentDeliveryScheduleQuery = new GetPurchaseOrderRecentDeliveryScheduleQuery
            {
            };
            var serviceResponse = await _mediator.Send(getPurchaseOrderRecentDeliveryScheduleQuery);
            return Ok(serviceResponse);
        }

        /// <summary>
        /// Get Purchase order profit and loss Report
        /// </summary>
        /// <param name="purchaseOrderResource"></param>
        /// <returns></returns>
        [HttpGet("items/profitLoss")]
        public async Task<IActionResult> GetPurchaseOrderProfitLossReport([FromQuery] PurchaseOrderResource purchaseOrderResource)
        {
            var getSaleOrderProfitLossCommand = new GetPurchaseOrderProfitLossCommand
            {
                FromDate = purchaseOrderResource.FromDate.Value,
                ToDate = purchaseOrderResource.ToDate.Value,
            };

            var response = await _mediator.Send(getSaleOrderProfitLossCommand);
            return Ok(response);
        }
    }
}
