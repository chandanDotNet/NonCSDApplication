using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using POS.API.Helpers;
using POS.Data.Dto;
using POS.Data.Resources;
using POS.MediatR.PurchaseOrderPayment.Command;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POS.API.Controllers.PurchaseOrderPayment
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PurchaseOrderPaymentController : BaseController
    {
        public IMediator _mediator { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PurchaseOrderPaymentController"/> class.
        /// </summary>
        /// <param name="mediator">The mediator.</param>
        public PurchaseOrderPaymentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get Purchase Order payments.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ClaimCheck("PO_VIEW_PO_PAYMENTS")]
        [Produces("application/json", "application/xml", Type = typeof(List<PurchaseOrderPaymentDto>))]
        public async Task<IActionResult> GetAllPurchaseOrderPayments(Guid id)
        {
            var getAllPurchaseOrderPaymentsCommand = new GetAllPurchaseOrderPaymentsCommand
            {
                Id = id
            };
            var paymentDtos = await _mediator.Send(getAllPurchaseOrderPaymentsCommand);

            return Ok(paymentDtos);
        }

        /// <summary>
        /// Get Purchase Order payments Report.
        /// </summary>
        /// <returns></returns>
        [HttpGet("report")]
        [ClaimCheck("REP_PO_PAYMENT_REP")]
        [Produces("application/json", "application/xml", Type = typeof(List<PurchaseOrderPaymentDto>))]
        public async Task<IActionResult> GetAllPurchaseOrderPaymentsReport([FromQuery] PurchaseOrderResource purchaseOrderResource)
        {
            var getAllPurchaseOrderPaymentsReportCommand = new GetAllPurchaseOrderPaymentsReportCommand
            {
                PurchaseOrderResource = purchaseOrderResource
            };
            var purchaseOrderPayments = await _mediator.Send(getAllPurchaseOrderPaymentsReportCommand);

            var paginationMetadata = new
            {
                totalCount = purchaseOrderPayments.TotalCount,
                pageSize = purchaseOrderPayments.PageSize,
                skip = purchaseOrderPayments.Skip,
                totalPages = purchaseOrderPayments.TotalPages
            };

            Response.Headers.Add("X-Pagination",
                    Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetadata));

            return Ok(purchaseOrderPayments);
        }

        /// <summary>
        /// Create Purchase Order payment.
        /// </summary>
        /// <param name="addPurchaseOrderPaymentCommand"></param>
        /// <returns></returns>
        [HttpPost, DisableRequestSizeLimit]
        [ClaimCheck("PO_ADD_PO_PAYMENT")]
        [Produces("application/json", "application/xml", Type = typeof(PurchaseOrderDto))]
        public async Task<IActionResult> CreatePurchaseOrderPayment(AddPurchaseOrderPaymentCommand addPurchaseOrderPaymentCommand)
        {
            var result = await _mediator.Send(addPurchaseOrderPaymentCommand);
            return ReturnFormattedResponse(result);
        }

        /// <summary>
        /// Delete Purchase Order payment.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ClaimCheck("PO_DELETE_PO_PAYMENT")]
        public async Task<IActionResult> DeletePurchaseOrderPayment(Guid id)
        {
            var deletePurchaseOrderCommand = new DeletePurchaseOrderPaymentCommand
            {
                Id = id
            };
            var response = await _mediator.Send(deletePurchaseOrderCommand);
            return ReturnFormattedResponse(response);
        }
    }
}
