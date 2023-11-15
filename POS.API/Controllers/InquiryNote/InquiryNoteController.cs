using POS.Data.Dto;
using POS.MediatR.CommandAndQuery;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using POS.API.Helpers;

namespace POS.API.Controllers.InquiryNote
{
    [Route("api/[controller]")]
    [ApiController]
    public class InquiryNoteController : BaseController
    {
        private readonly IMediator _mediator;
        /// <summary>
        /// Initializes a new instance of the <see cref="InquiryNoteController"/> class.
        /// </summary>
        /// <param name="mediator">The mediator.</param>
        public InquiryNoteController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Gets the Inquiry Notes .
        /// </summary>
        /// <returns></returns>
        [HttpGet("{inquiryId}")]
        [ClaimCheck("INQ_VIEW_INQUIRIES")]
        [Produces("application/json", "application/xml", Type = typeof(List<InquiryNoteDto>))]
        public async Task<IActionResult> GetInquiryNotes(Guid inquiryId)
        {
            var query = new GetInquiryNotesQuery { InquiryId = inquiryId };
            var result = await _mediator.Send(query);
            return Ok(result);
        }
        /// <summary>
        /// Inserts the Inquiry Note
        /// </summary>
        /// <param name="addInquiryNoteCommand">The add Inquiry Note command.</param>
        /// <returns></returns>
        [HttpPost]
        [ClaimCheck("INQ_ADD_INQUIRY")]
        [Produces("application/json", "application/xml", Type = typeof(InquiryNoteDto))]
        public async Task<IActionResult> AddInquiryNote([FromBody] AddInquiryNoteCommand addInquiryNoteCommand)
        {
            var result = await _mediator.Send(addInquiryNoteCommand);
            return ReturnFormattedResponse(result);
        }

        /// <summary>
        /// Updates the Inquiry Note By Id.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="updateInquiryNoteCommand">The update Inquiry By Id command.</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ClaimCheck("INQ_UPDATE_INQUIRY")]
        [Produces("application/json", "application/xml", Type = typeof(InquiryNoteDto))]
        public async Task<IActionResult> UpdateInquiryNote(Guid id, [FromBody] UpdateInquiryNoteCommand updateInquiryNoteCommand)
        {
            updateInquiryNoteCommand.Id = id;
            var result = await _mediator.Send(updateInquiryNoteCommand);
            return ReturnFormattedResponse(result);
        }

        /// <summary>
        /// Deletes the Inquiry Note By Id.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ClaimCheck("INQ_DELETE_INQUIRY")]
        public async Task<IActionResult> DeleteInquiryNote(Guid id)
        {
            var command = new DeleteInquiryNoteCommand() { Id = id };
            var result = await _mediator.Send(command);
            return ReturnFormattedResponse(result);
        }
    }
}
