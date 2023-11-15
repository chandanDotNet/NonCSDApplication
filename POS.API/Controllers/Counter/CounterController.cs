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


        



        


        public class ResponseData1
        {
            public bool status { get; set; }
            public int StatusCode { get; set; }
            public string message { get; set; }
            public IList<ProductDto> Data { get; set; }
        }


        //public ResponseData1 CustomReturnFormattedRespons(bool Status,int StatusCode,string Message)
        //{
        //    ResponseData1 responseData = new ResponseData1();
        //    responseData.status = Status;
        //    responseData.StatusCode = StatusCode;
        //    responseData.message = Message;

        //    return responseData;
        //}

    }
}
