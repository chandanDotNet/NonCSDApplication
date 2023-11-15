using POS.Data.Dto;
using POS.MediatR.CommandAndQuery;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using POS.MediatR.Brand.Command;

namespace POS.API.Controllers.Brand
{
    [Route("api")]
    [ApiController]
    [Authorize]
    public class BrandController : BaseController
    {
        public IMediator _mediator { get; set; }

        public BrandController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get Brand.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("Brand/{id}", Name = "GetBrand")]
        [Produces("application/json", "application/xml", Type = typeof(BrandDto))]
        public async Task<IActionResult> GetBrand(Guid id)
        {
            var getBrandCommand = new GetBrandCommand { Id = id };
            var result = await _mediator.Send(getBrandCommand);
            return ReturnFormattedResponse(result);
        }

        /// <summary>
        /// Get Brands.
        /// </summary>
        /// <returns></returns>
        [HttpGet("Brands")]
        [Produces("application/json", "application/xml", Type = typeof(List<BrandDto>))]
        public async Task<IActionResult> GetBrands()
        {
            var getAllBrandCommand = new GetAllBrandCommand { };
            var result = await _mediator.Send(getAllBrandCommand);
            return Ok(result);
        }

        /// <summary>
        /// Create Brand.
        /// </summary>
        /// <param name="addBrandCommand"></param>
        /// <returns></returns>
        [HttpPost("Brand")]
        [Produces("application/json", "application/xml", Type = typeof(BrandDto))]
        public async Task<IActionResult> AddBrand(AddBrandCommand addBrandCommand)
        {
            var response = await _mediator.Send(addBrandCommand);
            if (!response.Success)
            {
                return ReturnFormattedResponse(response);
            }
            return CreatedAtAction("GetBrand", new { id = response.Data.Id }, response.Data);
        }

        /// <summary>
        /// Update Brand.
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="updateBrandCommand"></param>
        /// <returns></returns>
        [HttpPut("Brand/{Id}")]
        [Produces("application/json", "application/xml", Type = typeof(BrandDto))]
        public async Task<IActionResult> UpdateBrand(Guid Id, UpdateBrandCommand updateBrandCommand)
        {
            updateBrandCommand.Id = Id;
            var result = await _mediator.Send(updateBrandCommand);
            return ReturnFormattedResponse(result);

        }

        /// <summary>
        /// Delete Brand.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpDelete("Brand/{Id}")]
        public async Task<IActionResult> DeleteBrand(Guid Id)
        {
            var deleteBrandCommand = new DeleteBrandCommand { Id = Id };
            var result = await _mediator.Send(deleteBrandCommand);
            return ReturnFormattedResponse(result);
        }
    }
}
