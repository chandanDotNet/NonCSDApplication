using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using POS.MediatR.Currency.Commands;
using System.Threading.Tasks;

namespace POS.API.Controllers.Currency
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CurrencyController : ControllerBase
    {
        private IMediator _mediator;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mediator"></param>
        public CurrencyController(
            IMediator mediator)
        {
            _mediator = mediator;
        }


        /// <summary>
        /// Get All Currencies.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetCurrencies()
        {
            var getAllInquiryQuery = new GetAllCurrencyCommand();
            var currencies = await _mediator.Send(getAllInquiryQuery);
            return Ok(currencies);
        }
    }
}
