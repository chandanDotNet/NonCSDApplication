using POS.MediatR.CommandAndQuery;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using POS.Data.Dto;
using POS.MediatR.Category.Commands;

namespace POS.API.Controllers
{
    /// <summary>
    /// Category Controller
    /// </summary>
    [Route("api")]
    [ApiController]
    [Authorize]
    public class ProductCategoryController : BaseController
    {
        private readonly IMediator _mediator;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mediator"></param>
        public ProductCategoryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get all Product Categories
        /// </summary>
        /// <param name="getAllProductCategoriesQuery"></param>
        /// <returns></returns>
        [HttpGet]
        [HttpGet("ProductCategories")]
        [Produces("application/json", "application/xml", Type = typeof(List<ProductCategoryDto>))]
        public async Task<IActionResult> GetProductCategories([FromQuery] GetAllProductCategoriesQuery getAllProductCategoriesQuery)
        {
            var result = await _mediator.Send(getAllProductCategoriesQuery);
            return Ok(result);
        }

        /// <summary>
        /// Get Prodct Category.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("ProductCategory/{id}")]
        [Produces("application/json", "application/xml", Type = typeof(ProductCategoryDto))]
        public async Task<IActionResult> GetProductCategory(Guid id)
        {
            var getProductCategoryQuery = new GetProductCategoryByIdQuery { Id = id };
            var result = await _mediator.Send(getProductCategoryQuery);
            return ReturnFormattedResponse(result);
        }

        /// <summary>
        /// Create Product Category.
        /// </summary>
        /// <param name="addProductCategoryCommand"></param>
        /// <returns></returns>
        [HttpPost("ProductCategory")]
        [Produces("application/json", "application/xml", Type = typeof(ProductCategoryDto))]
        public async Task<IActionResult> AddProductCategory(AddProductCategoryCommand addProductCategoryCommand)
        {
            var response = await _mediator.Send(addProductCategoryCommand);
            if (!response.Success)
            {
                return ReturnFormattedResponse(response);
            }
            return CreatedAtAction("GetProductCategory", new { id = response.Data.Id }, response.Data);
        }

        /// <summary>
        /// Update Product Category.
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="updateProductCategoryCommand"></param>
        /// <returns></returns>
        [HttpPut("ProductCategory/{Id}")]
        [Produces("application/json", "application/xml", Type = typeof(ProductCategoryDto))]
        public async Task<IActionResult> UpdateProductCategory(Guid Id, UpdateProductCategoryCommand updateProductCategoryCommand)
        {
            updateProductCategoryCommand.Id = Id;
            var result = await _mediator.Send(updateProductCategoryCommand);
            return ReturnFormattedResponse(result);

        }

        /// <summary>
        /// Delete Product Category.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpDelete("ProductCategory/{Id}")]
        public async Task<IActionResult> DeleteProductCategory(Guid Id)
        {
            var deleteProductCategoryCommand = new DeleteProductCategoryCommand { Id = Id };
            var result = await _mediator.Send(deleteProductCategoryCommand);
            return ReturnFormattedResponse(result);
        }
    }
}
