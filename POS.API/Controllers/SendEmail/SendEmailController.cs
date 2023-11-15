using POS.MediatR.CommandAndQuery;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POS.API.Controllers
{
    /// <summary>
    /// Supplier Controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SendEmailController : BaseController
    {
        public readonly IMediator _mediator;
        private readonly ILogger<SendEmailController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="SendEmailController"/> class.
        /// </summary>
        /// <param name="mediator">The mediator.</param>
        public SendEmailController(
            IMediator mediator,
             ILogger<SendEmailController> logger

              )
        {
            _mediator = mediator;
            _logger = logger;

        }
        /// <summary>
        /// Get All Suppliers
        /// </summary>
        /// <param name="supplierResource"></param>
        /// <returns></returns>

        [HttpPost("suppliers")]
        public async Task<IActionResult> SendEmailSuppliers([FromBody] AddSendEmailSuppliersCommand addSendEmailSuppliersCommand)
        {
            var result = await _mediator.Send(addSendEmailSuppliersCommand);
            return Ok(result);
        }
    }
}
