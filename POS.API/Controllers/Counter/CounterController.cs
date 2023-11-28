using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using POS.API.Helpers;
using POS.Data.Dto;
using POS.MediatR.Counter.Commands;
using System.Threading.Tasks;
using POS.MediatR.CommandAndQuery;
using System;
using System.Collections.Generic;
using POS.MediatR.City.Commands;
using POS.MediatR.Country.Commands;
using POS.Data.Resources;
using POS.MediatR.Product.Command;
using POS.MediatR;
using Newtonsoft.Json;
using Azure;
using POS.Helper;
using POS.Data;

namespace POS.API.Controllers.Counter
{
    [Route("api")]
    [ApiController]
    [Authorize]
    public class CounterController : BaseController
    {
        public IMediator _mediator { get; set; }
        public CounterController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Create Counter.
        /// </summary>
        /// <param name="addCounterCommand"></param>
        /// <returns></returns>
        [HttpPost("Counter")]
        [Produces("application/json", "application/xml", Type = typeof(CounterDto))]
        //[ClaimCheck("SETT_MANAGE_COUNTER")]
        public async Task<IActionResult> AddCounter([FromBody] AddCounterCommand addCounterCommand)
        {
            var result = await _mediator.Send(addCounterCommand);
            return ReturnFormattedResponse(result);
        }


        /// <summary>
        /// Get Counters.
        /// </summary>
        /// <returns></returns>
        [HttpGet("Counters")]
        [Produces("application/json", "application/xml", Type = typeof(List<CounterDto>))]
        public async Task<IActionResult> GetCounters()
        {
            var getAllCounterCommand = new GetAllCounterCommand { };
            var result = await _mediator.Send(getAllCounterCommand);
            return Ok(result);
        }


        /// <summary>
        /// Get Counter.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("Counter/{id}", Name = "GetCounter")]
        [Produces("application/json", "application/xml", Type = typeof(CounterDto))]
        public async Task<IActionResult> GetCounter(Guid id)
        {
            var getCounterCommand = new GetCounterCommand { Id = id };
            var result = await _mediator.Send(getCounterCommand);
            return ReturnFormattedResponse(result);
        }





    }
}
