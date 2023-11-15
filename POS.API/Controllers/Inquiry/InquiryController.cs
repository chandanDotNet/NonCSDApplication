using POS.Data.Resources;
using POS.MediatR.CommandAndQuery;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Text.Json;
using System.Threading.Tasks;
using POS.MediatR.Inquiry.Commands;
using POS.API.Helpers;

namespace POS.API.Controllers
{
    /// <summary>
    /// Inquiry
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class InquiryController : BaseController
    {
        private IMediator _mediator;
        private readonly ILogger<InquiryController> _logger;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="logger"></param>
        public InquiryController(
            IMediator mediator,
            ILogger<InquiryController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }
        /// <summary>
        /// Add Inquiry
        /// </summary>
        /// <param name="addInquiryCommand"></param>
        /// <returns></returns>
        [HttpPost]
        [ClaimCheck("INQ_ADD_INQUIRY")]
        [AllowAnonymous]
        public async Task<IActionResult> AddInquery([FromBody] AddInquiryCommand addInquiryCommand)
        {
            var result = await _mediator.Send(addInquiryCommand);
            if (result.StatusCode != 200)
            {
                _logger.LogError(result.StatusCode,
                                JsonSerializer.Serialize(result), "");
                return StatusCode(result.StatusCode, result);
            }
            return CreatedAtRoute("GetInquiry", new { id = result.Data.Id }, result.Data);
        }
        /// <summary>
        /// Update Inquiry By Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updateInquiryCommand"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ClaimCheck("INQ_UPDATE_INQUIRY")]
        public async Task<IActionResult> UpdateInquiry(Guid id, [FromBody] UpdateInquiryCommand updateInquiryCommand)
        {
            updateInquiryCommand.Id = id;
            var result = await _mediator.Send(updateInquiryCommand);
            if (result.StatusCode != 200)
            {
                _logger.LogError(result.StatusCode,
                                JsonSerializer.Serialize(result), "");
                return StatusCode(result.StatusCode, result);
            }
            return ReturnFormattedResponse(result);
        }

        /// <summary>
        /// Get Inquiry 
        /// </summary>
        /// <param name="inquiryResource"></param>
        /// <returns></returns>

        [HttpGet]
        [ClaimCheck("INQ_VIEW_INQUIRIES")]
        public async Task<IActionResult> GetInquiries([FromQuery] InquiryResource inquiryResource)
        {

            var getAllInquiryQuery = new GetAllInquiryQuery
            {
                InquiryResource = inquiryResource
            };
            var result = await _mediator.Send(getAllInquiryQuery);

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
        /// Gets the inquiry by Id.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "GetInquiry")]
        [ClaimCheck("INQ_VIEW_INQUIRIES")]
        public async Task<IActionResult> GetInquiry(Guid id)
        {
            var query = new GetInquiryQuery { Id = id };
            var result = await _mediator.Send(query);
            return ReturnFormattedResponse(result);
        }

        /// <summary>
        /// Deletes the inquiry.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ClaimCheck("INQ_DELETE_INQUIRY")]
        public async Task<IActionResult> DeleteInquiry(Guid id)
        {
            var command = new DeleteInquiryCommand() { Id = id };
            var result = await _mediator.Send(command);
            return ReturnFormattedResponse(result);
        }

        /// <summary>
        /// Gets the Products by inquiry Id.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpGet("{id}/products")]
        public async Task<IActionResult> GetProductsByInquiryId(Guid id)
        {
            var query = new GetInquiryProductsCommand { Id = id };
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}
