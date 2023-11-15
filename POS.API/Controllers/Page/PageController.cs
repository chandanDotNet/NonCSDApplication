using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using POS.Data.Dto;
using POS.MediatR.CommandAndQuery;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace POS.API.Controllers
{
    /// <summary>
    /// Page
    /// </summary>
    [Route("api")]
    [ApiController]
    [Authorize]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class PageController : BaseController
    {
        public IMediator _mediator { get; set; }
        /// <summary>
        /// Page
        /// </summary>
        /// <param name="mediator"></param>
        public PageController(IMediator mediator)
        {
            _mediator = mediator;
        }
        /// <summary>
        /// Get Page By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("Page/{id}", Name = "GetPage")]
        [Produces("application/json", "application/xml", Type = typeof(PageDto))]
        public async Task<IActionResult> GetPage(Guid id)
        {
            var getPageQuery = new GetPageQuery { Id = id };
            var result = await _mediator.Send(getPageQuery);
            return ReturnFormattedResponse(result);
        }
        /// <summary>
        /// Get All Pages
        /// </summary>
        /// <returns>Test</returns>
        /// <response code="200">Returns the newly created item</response>
        [HttpGet("Pages")]
        [Produces("application/json", "application/xml", Type = typeof(List<PageDto>))]
        public async Task<IActionResult> GetPages()
        {
            var getAllPageQuery = new GetAllPageQuery { };
            var result = await _mediator.Send(getAllPageQuery);
            return Ok(result);
        }
        /// <summary>
        /// Create a Page
        /// </summary>
        /// <param name="addPageCommand"></param>
        /// <returns></returns>
        [HttpPost("Page")]
        [Produces("application/json", "application/xml", Type = typeof(PageDto))]
        public async Task<IActionResult> AddPage(AddPageCommand addPageCommand)
        {
            var result = await _mediator.Send(addPageCommand);
            if (!result.Success)
            {
                return ReturnFormattedResponse(result);
            }
            return CreatedAtAction("GetPage", new { id = result.Data.Id }, result.Data);
        }
        /// <summary>
        /// Update Page By Id
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="updatePageCommand"></param>
        /// <returns></returns>
        [HttpPut("Page/{Id}")]
        [Produces("application/json", "application/xml", Type = typeof(PageDto))]
        public async Task<IActionResult> UpdatePage(Guid Id, UpdatePageCommand updatePageCommand)
        {
            updatePageCommand.Id = Id;
            var result = await _mediator.Send(updatePageCommand);
            return ReturnFormattedResponse(result);
        }
        /// <summary>
        /// Delete Page By Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpDelete("Page/{Id}")]
        public async Task<IActionResult> DeletePage(Guid Id)
        {
            var deletePageCommand = new DeletePageCommand { Id = Id };
            var result = await _mediator.Send(deletePageCommand);
            return ReturnFormattedResponse(result);
        }
    }
}
