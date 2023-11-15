using POS.MediatR.CommandAndQuery;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace POS.API.Controllers.Testimonials
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TestimonialsController : BaseController
    {
        private readonly IMediator _mediator;
        public TestimonialsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Gets the testimonials.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetTestimonials()
        {
            var query = new GetAllTestimonialsQuery();
            var testimonials = await _mediator.Send(query);
            return Ok(testimonials);
        }

        /// <summary>
        /// Adds the testimonial.
        /// </summary>
        /// <param name="testimonialsCommand">The testimonials command.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> AddTestimonial([FromBody] AddTestimonialsCommand testimonialsCommand)
        {
            var result = await _mediator.Send(testimonialsCommand);
            if (!result.Success)
            {
                return ReturnFormattedResponse(result);
            }
            return CreatedAtRoute("GetTestimonial", new { id = result.Data.Id }, result.Data);
        }

        /// <summary>
        /// Updates the testimonial.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="updateTestimonialsCommand">The update testimonials command.</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTestimonial(Guid id, [FromBody] UpdateTestimonialsCommand updateTestimonialsCommand)
        {
            updateTestimonialsCommand.Id = id;
            var result = await _mediator.Send(updateTestimonialsCommand);
            return ReturnFormattedResponse(result);
        }

        /// <summary>
        /// Gets the testimonial.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "GetTestimonial")]
        public async Task<IActionResult> GetTestimonial(Guid id)
        {
            var query = new GetTestimonialsQuery { Id = id };
            var result = await _mediator.Send(query);
            return ReturnFormattedResponse(result);
        }

        /// <summary>
        /// Deletes the testimonial.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTestimonial(Guid id)
        {
            var command = new DeleteTestimonialsCommand() { Id = id };
            var result = await _mediator.Send(command);
            return ReturnFormattedResponse(result);
        }
    }
}
