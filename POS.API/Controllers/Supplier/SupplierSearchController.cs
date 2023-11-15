using POS.MediatR.CommandAndQuery;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POS.API.Controllers.Supplier
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SupplierSearchController : ControllerBase
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="SupplierSearchController"/> class.
        /// </summary>
        /// <param name="mediator">The mediator.</param>
        public SupplierSearchController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Searches the suppliers.
        /// </summary>
        /// <param name="searchQuery">The search query.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns></returns>
        [HttpGet(Name = "SearchSuppliers")]
        public async Task<IActionResult> SearchSuppliers(string searchQuery, int pageSize)
        {
            var query = new SearchSupplierQuery
            {
                PageSize = pageSize,
                SearchQuery = searchQuery
            };
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}
