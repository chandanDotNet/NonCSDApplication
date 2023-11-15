using POS.MediatR.CommandAndQuery;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace POS.API.Controllers.NewsletterSubscriber
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsletterSubscriberController : ControllerBase
    {
        private readonly IMediator _mediator;
        public NewsletterSubscriberController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Creates the newsletter subscriber.
        /// </summary>
        /// <param name="addNewsletterSubscriberCommand">The add newsletter subscriber command.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateNewsletterSubscriber([FromBody] AddNewsletterSubscriberCommand addNewsletterSubscriberCommand)
        {
            await _mediator.Send(addNewsletterSubscriberCommand);
            return Ok();
        }
    }
}
