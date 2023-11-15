using POS.Data.Resources;
using POS.MediatR.CommandAndQuery;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace POS.API.Controllers.ContactUs
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactUsController : BaseController
    {
        private readonly IMediator _mediator;
        public ContactUsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Creates the specified add contact us.
        /// </summary>
        /// <param name="addContactUsCommand">The add contact us command.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddContactUsCommand addContactUsCommand)
        {
            var result = await _mediator.Send(addContactUsCommand);
            return ReturnFormattedResponse(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetContactUsList([FromQuery] ContactUsResource contactUsResource)
        {

            var getAllContactUsQuery = new GetAllContactUsQuery
            {
                ContactUsResource = contactUsResource
            };
            var result = await _mediator.Send(getAllContactUsQuery);

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

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleteContactUsCommand = new DeleteContactUsCommand
            {
                Id = id
            };
            var result = await _mediator.Send(deleteContactUsCommand);
            return ReturnFormattedResponse(result);
        }

    }
}
