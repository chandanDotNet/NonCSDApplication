using POS.Data.Dto;
using POS.Data.Entities;
using POS.MediatR.CommandAndQuery;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using POS.API.Helpers;

namespace POS.API.Controllers
{
    [Route("api")]
    [ApiController]
    [Authorize]
    public class InquiryStatusController : BaseController
    {
        private readonly IMediator _mediator;
        /// <summary>
        /// Initializes a new instance of the <see cref="InquiryStatusController"/> class.
        /// </summary>
        /// <param name="mediator">The mediator.</param>
        public InquiryStatusController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get All Inquiry Status.
        /// </summary>
        /// <returns></returns>
        [HttpGet("InquiryStatuses")]
        [ClaimCheck("INQ_MANAGE_INQ_STATUS")]
        [Produces("application/json", "application/xml", Type = typeof(List<InquiryStatus>))]
        public async Task<IActionResult> GetAllInquiryStatus()
        {
            var query = new GetAllInquiryStatusQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Get Inquiry status.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("InquiryStatus/{id}")]
        [ClaimCheck("INQ_MANAGE_INQ_STATUS")]
        [Produces("application/json", "application/xml", Type = typeof(InquiryStatusDto))]
        public async Task<IActionResult> GetInquiryStatus(Guid id)
        {
            var getInquiryStatusQuery = new GetInquiryStatusQuery { Id = id };
            var result = await _mediator.Send(getInquiryStatusQuery);
            return ReturnFormattedResponse(result);
        }

        /// <summary>
        /// Create Inquiry Status.
        /// </summary>
        /// <param name="addInquiryStatusCommand"></param>
        /// <returns></returns>
        [HttpPost("InquiryStatus")]
        [ClaimCheck("INQ_MANAGE_INQ_STATUS")]
        [Produces("application/json", "application/xml", Type = typeof(InquiryStatusDto))]
        public async Task<IActionResult> AddInquiryStatus(AddInquiryStatusCommand addInquiryStatusCommand)
        {
            var response = await _mediator.Send(addInquiryStatusCommand);
            if (!response.Success)
            {
                return ReturnFormattedResponse(response);
            }
            return CreatedAtAction("GetInquiryStatus", new { id = response.Data.Id }, response.Data);
        }
        
        /// <summary>
        /// Update Inquiry status.
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="updateInquiryStatusCommand"></param>
        /// <returns></returns>
        [HttpPut("InquiryStatus/{Id}")]
        [ClaimCheck("INQ_MANAGE_INQ_STATUS")]
        [Produces("application/json", "application/xml", Type = typeof(InquiryStatusDto))]
        public async Task<IActionResult> UpdateInquiryStatus(Guid Id, UpdateInquiryStatusCommand updateInquiryStatusCommand)
        {
            updateInquiryStatusCommand.Id = Id;
            var result = await _mediator.Send(updateInquiryStatusCommand);
            return ReturnFormattedResponse(result);

        }
       
        /// <summary>
        /// Delete Inquiry Status.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpDelete("InquiryStatus/{Id}")]
        [ClaimCheck("INQ_MANAGE_INQ_STATUS")]
        public async Task<IActionResult> DeleteInquiryStatus(Guid Id)
        {
            var deleteInquiryStatusCommand = new DeleteInquiryStatusCommand { Id = Id };
            var result = await _mediator.Send(deleteInquiryStatusCommand);
            return ReturnFormattedResponse(result);
        }
    }
}
