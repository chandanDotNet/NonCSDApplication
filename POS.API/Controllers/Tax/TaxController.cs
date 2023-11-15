using POS.Data.Dto;
using POS.MediatR.CommandAndQuery;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using POS.MediatR.Tax.Commands;

namespace POS.API.Controllers.Tax
{
    [Route("api")]
    [ApiController]
    [Authorize]
    public class TaxController : BaseController
    {
        public IMediator _mediator { get; set; }

        public TaxController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get Tax.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("Tax/{id}", Name = "GetTax")]
        [Produces("application/json", "application/xml", Type = typeof(TaxDto))]
        public async Task<IActionResult> GetTax(Guid id)
        {
            var getTaxCommand = new GetTaxCommand { Id = id };
            var result = await _mediator.Send(getTaxCommand);
            return ReturnFormattedResponse(result);
        }

        /// <summary>
        /// Get Tax.
        /// </summary>
        /// <returns></returns>
        [HttpGet("Taxes")]
        [Produces("application/json", "application/xml", Type = typeof(List<TaxDto>))]
        public async Task<IActionResult> GetTaxes()
        {
            var getAllTaxCommand = new GetAllTaxCommand { };
            var result = await _mediator.Send(getAllTaxCommand);
            return Ok(result);
        }

        /// <summary>
        /// Create Tax.
        /// </summary>
        /// <param name="addTaxCommand"></param>
        /// <returns></returns>
        [HttpPost("Tax")]
        [Produces("application/json", "application/xml", Type = typeof(TaxDto))]
        public async Task<IActionResult> AddTax(AddTaxCommand addTaxCommand)
        {
            var response = await _mediator.Send(addTaxCommand);
            if (!response.Success)
            {
                return ReturnFormattedResponse(response);
            }
            return CreatedAtAction("GetTax", new { id = response.Data.Id }, response.Data);
        }

        /// <summary>
        /// Update Tax.
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="updateTaxCommand"></param>
        /// <returns></returns>
        [HttpPut("Tax/{Id}")]
        [Produces("application/json", "application/xml", Type = typeof(TaxDto))]
        public async Task<IActionResult> UpdateTax(Guid Id, UpdateTaxCommand updateTaxCommand)
        {
            updateTaxCommand.Id = Id;
            var result = await _mediator.Send(updateTaxCommand);
            return ReturnFormattedResponse(result);

        }

        /// <summary>
        /// Delete Tax.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpDelete("Tax/{Id}")]
        public async Task<IActionResult> DeleteTax(Guid Id)
        {
            var deleteTaxCommand = new DeleteTaxCommand { Id = Id };
            var result = await _mediator.Send(deleteTaxCommand);
            return ReturnFormattedResponse(result);
        }
    }
}
