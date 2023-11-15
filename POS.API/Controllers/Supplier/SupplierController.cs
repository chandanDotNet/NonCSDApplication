using POS.Data.Resources;
using POS.MediatR.CommandAndQuery;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Text.Json;
using System.Threading.Tasks;
using POS.API.Helpers;
using POS.MediatR.Supplier.Commands;

namespace POS.API.Controllers
{
    /// <summary>
    /// Supplier Controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SupplierController : BaseController
    {
        public readonly IMediator _mediator;
        private readonly ILogger<SupplierController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="SupplierController"/> class.
        /// </summary>
        /// <param name="mediator">The mediator.</param>
        public SupplierController(IMediator mediator,
             ILogger<SupplierController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        /// <summary>
        /// Get All Suppliers
        /// </summary>
        /// <param name="supplierResource"></param>
        /// <returns></returns>

        [HttpGet(Name = "GetSuppliers")]
        [ClaimCheck("SUPP_VIEW_SUPPLIERS")]
        public async Task<IActionResult> GetSuppliers([FromQuery] SupplierResource supplierResource)
        {
            var getAllSupplierQuery = new GetAllSupplierQuery
            {
                SupplierResource = supplierResource
            };
            var result = await _mediator.Send(getAllSupplierQuery);

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
        /// Get Supplier by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "GetSupplier")]
        [ClaimCheck("SUPP_VIEW_SUPPLIERS")]
        public async Task<IActionResult> GetSupplier(Guid id)
        {
            var getSupplierQuery = new GetSupplierQuery
            {
                Id = id
            };

            var result = await _mediator.Send(getSupplierQuery);
            if (result.StatusCode != 200)
            {
                _logger.LogError(result.StatusCode,
                                JsonSerializer.Serialize(result), "");
                return StatusCode(result.StatusCode, result);
            }
            return ReturnFormattedResponse(result);
        }

        /// <summary>
        /// Add Supplier
        /// </summary>
        /// <param name="addSupplierCommand"></param>
        /// <returns></returns>
        [HttpPost, DisableRequestSizeLimit]
        [ClaimCheck("SUPP_ADD_SUPPLIER")]
        public async Task<IActionResult> AddSupplier([FromBody] AddSupplierCommand addSupplierCommand)
        {
            var result = await _mediator.Send(addSupplierCommand);
            if (result.StatusCode != 200)
            {
                _logger.LogError(result.StatusCode,
                                JsonSerializer.Serialize(result), "");
                return StatusCode(result.StatusCode, result);
            }
            return CreatedAtRoute("GetSupplier",
                 new { id = result.Data.Id },
                 result.Data);
        }
        /// <summary>
        /// Update Supplier By Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updateSupplierCommand"></param>
        /// <returns></returns>
        [HttpPut("{id}"), DisableRequestSizeLimit]
        [ClaimCheck("SUPP_UPDATE_SUPPLIER")]
        public async Task<IActionResult> UpdateSupplier(Guid id, [FromBody] UpdateSupplierCommand updateSupplierCommand)
        {
            updateSupplierCommand.Id = id;
            var result = await _mediator.Send(updateSupplierCommand);
            if (result.StatusCode != 200)
            {
                _logger.LogError(result.StatusCode,
                                JsonSerializer.Serialize(result), "");
                return StatusCode(result.StatusCode, result);
            }
            return NoContent();
        }
        /// <summary>
        /// Delete Supplier By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ClaimCheck("SUPP_DELETE_SUPPLIER")]
        public async Task<IActionResult> DeleteSupplier(Guid id)
        {
            var deleteSupplierCommand = new DeleteSupplierCommand { Id = id };
            var result = await _mediator.Send(deleteSupplierCommand);
            if (result.StatusCode != 200)
            {
                _logger.LogError(result.StatusCode,
                                JsonSerializer.Serialize(result.Errors), "");
                return StatusCode(result.StatusCode, result.Errors);
            }
            return ReturnFormattedResponse(result);
        }

        /// <summary>
        /// Get Latest Register Suppliers
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetNewSupplier")]
        [ClaimCheck("SUPP_VIEW_SUPPLIERS")]
        public async Task<IActionResult> GetNewSupplier()
        {
            var result = await _mediator.Send(new GetNewSupplierQuery { });
            return Ok(result);
        }

        /// <summary>
        /// Get Supplier Payment.
        /// </summary>
        /// <param name="supplierResource"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetSupplierPayment")]
        //[ClaimCheck("SUPP_VIEW_SUPPLIERS")]
        public async Task<IActionResult> GetSupplierPayment([FromQuery] SupplierResource supplierResource)
        {
            var result = await _mediator.Send(new GetSupplierPaymentsQuery
            {
                SupplierResource = supplierResource
            });

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
