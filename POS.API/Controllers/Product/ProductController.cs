using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using POS.API.Helpers;
using POS.Data.Dto;
using POS.Data.Resources;
using POS.MediatR;
using POS.MediatR.Product.Command;
using System;
using System.Threading.Tasks;

namespace POS.API.Controllers.Product
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductController : BaseController
    {
        private readonly IMediator _mediator;
        public ProductController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get All Products.
        /// </summary>
        /// <param name="productResource"></param>
        /// <returns></returns>
        [HttpGet(Name = "GetProducts")]
        [ClaimCheck("PRO_VIEW_PRODUCTS")]
        public async Task<IActionResult> GetProducts([FromQuery] ProductResource productResource)
        {
            var getAllProductCommand = new GetAllProductCommand
            {
                ProductResource = productResource
            };
            var result = await _mediator.Send(getAllProductCommand);

            var paginationMetadata = new
            {
                totalCount = result.TotalCount,
                pageSize = result.PageSize,
                skip = result.Skip,
                totalPages = result.TotalPages
            };
            Response.Headers.Add("X-Pagination",
                Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetadata));
            return Ok(result);
        }

        /// <summary>
        /// Get All Products By .
        /// </summary>
        /// <returns></returns>
        [HttpGet("bywarehouse")]
        [AllowAnonymous]
        public async Task<IActionResult> GetProductByWarehouse()
        {
            var productByWarehouseCommand = new ProductByWarehouseCommand();
            var result = await _mediator.Send(productByWarehouseCommand);
            return Ok(result.Data);
        }


        /// <summary>
        /// Get Product.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ClaimCheck("PRO_VIEW_PRODUCTS")]
        [Produces("application/json", "application/xml", Type = typeof(ProductDto))]
        public async Task<IActionResult> GetProduct(Guid id)
        {
            var getProductCommand = new GetProductCommand { Id = id };
            var result = await _mediator.Send(getProductCommand);
            return ReturnFormattedResponse(result);
        }

        /// <summary>
        /// Add Product.
        /// </summary>
        /// <param name="addProductCommand"></param>
        /// <returns></returns>
        [HttpPost]
        [ClaimCheck("PRO_ADD_PRODUCT")]
        [DisableRequestSizeLimit]
        [Produces("application/json", "application/xml", Type = typeof(ProductDto))]
        public async Task<IActionResult> AddProduct(AddProductCommand addProductCommand)
        {
            var response = await _mediator.Send(addProductCommand);
            return ReturnFormattedResponse(response);
        }


        /// <summary>
        /// Update Product.
        /// </summary>
        /// <param name="updateProductCommand"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ClaimCheck("PRO_UPDATE_PRODUCT")]
        [DisableRequestSizeLimit]
        [Produces("application/json", "application/xml", Type = typeof(ProductDto))]
        public async Task<IActionResult> UpdateProduct(Guid id, UpdateProductCommand updateProductCommand)
        {
            updateProductCommand.Id = id;
            var response = await _mediator.Send(updateProductCommand);
            return ReturnFormattedResponse(response);
        }

        /// <summary>
        /// Delete Product.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ClaimCheck("PRO_DELETE_PRODUCT")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            var command = new DeleteProductCommand() { Id = id };
            var result = await _mediator.Send(command);
            return ReturnFormattedResponse(result);
        }

        /// <summary>
        /// Get Products By Detail
        /// </summary>
        /// <returns></returns>
        [HttpGet("detail")]
        [ClaimCheck("PRO_VIEW_PRODUCTS")]
        [Produces("application/json", "application/xml", Type = typeof(ProductDto))]
        public async Task<IActionResult> GetProductsDetail()
        {
            var getProductsDetail = new GetProductsDetailCommand();
            var response = await _mediator.Send(getProductsDetail);
            return ReturnFormattedResponse(response);
        }
    }
}
