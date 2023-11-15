using POS.MediatR.CommandAndQuery;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using POS.Data.Dto;
using System;
using System.Collections.Generic;
using POS.MediatR.Country.Commands;
using Microsoft.AspNetCore.Authorization;
using POS.MediatR.Country.Command;
using POS.API.Helpers;

namespace POS.API.Controllers.Country
{
    [Route("api")]
    [ApiController]
    [Authorize]
    public class CountryController : BaseController
    {
        public IMediator _mediator { get; set; }

        public CountryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get Country.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("Country/{id}", Name = "GetCountry")]
        [ClaimCheck("SETT_MANAGE_COUNTRY")]
        [Produces("application/json", "application/xml", Type = typeof(CountryDto))]
        public async Task<IActionResult> GetCountry(Guid id)
        {
            var getCountryCommand = new GetCountryQuery { Id = id };
            var result = await _mediator.Send(getCountryCommand);
            return Ok(result);
        }

        /// <summary>
        /// Get Country.
        /// </summary>
        /// <returns></returns>
        [HttpGet("Countries")]
        [ClaimCheck("SETT_MANAGE_COUNTRY")]
        [Produces("application/json", "application/xml", Type = typeof(List<CountryDto>))]
        public async Task<IActionResult> GetCountries()
        {
            var getAllCountryCommand = new GetAllCountryCommand { };
            var result = await _mediator.Send(getAllCountryCommand);
            return Ok(result);
        }

        /// <summary>
        /// Create Country.
        /// </summary>
        /// <param name="addCountryCommand"></param>
        /// <returns></returns>
        [HttpPost("Country")]
        [ClaimCheck("SETT_MANAGE_COUNTRY")]
        [Produces("application/json", "application/xml", Type = typeof(CountryDto))]
        public async Task<IActionResult> AddCountry(AddCountryCommand addCountryCommand)
        {
            var response = await _mediator.Send(addCountryCommand);
            if (!response.Success)
            {
                return ReturnFormattedResponse(response);
            }
            return CreatedAtAction("GetCountry", new { id = response.Data.Id }, response.Data);
        }

        /// <summary>
        /// Update Country.
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="updateCountryCommand"></param>
        /// <returns></returns>
        [HttpPut("Country/{Id}")]
        [ClaimCheck("SETT_MANAGE_COUNTRY")]
        [Produces("application/json", "application/xml", Type = typeof(CountryDto))]
        public async Task<IActionResult> UpdateCountry(Guid Id, UpdateCountryCommand updateCountryCommand)
        {
            updateCountryCommand.Id = Id;
            var result = await _mediator.Send(updateCountryCommand);
            return ReturnFormattedResponse(result);

        }

        /// <summary>
        /// Delete Country.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpDelete("Country/{Id}")]
        [ClaimCheck("SETT_MANAGE_COUNTRY")]
        public async Task<IActionResult> DeleteCountry(Guid Id)
        {
            var deleteCountryCommand = new DeleteCountryCommand { Id = Id };
            var result = await _mediator.Send(deleteCountryCommand);
            return ReturnFormattedResponse(result);
        }
    }

}
