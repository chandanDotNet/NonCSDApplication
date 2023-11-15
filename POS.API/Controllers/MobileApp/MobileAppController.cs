using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using POS.Data.Dto;
using POS.Data;
using POS.MediatR.CommandAndQuery;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using static POS.API.Controllers.Counter.CounterController;
using System.Collections.Generic;
using System.Linq;
using POS.MediatR.Commands;
using POS.API.Helpers;
using System;
using POS.MediatR.Country.Command;
using POS.MediatR.Counter.Commands;
using Azure;
using Microsoft.AspNetCore.Authorization;
using POS.Data.Entities;
using POS.Data.Resources;
using POS.MediatR.Product.Command;
using POS.MediatR.CustomerAddress.Commands;

namespace POS.API.Controllers.MobileApp
{
    //[Route("api/[controller]")]
    [Route("api")]
    [ApiController]
    [Authorize]
    public class MobileAppController : BaseController
    {
        public IMediator _mediator { get; set; }
        public MobileAppController(IMediator mediator)
        {
            _mediator = mediator;
        }


        //
        /// <summary>
        /// Login customers.
        /// 
        /// </summary>
        /// <param name="customerResource">The customer resource.</param>
        /// <returns></returns>
        [HttpPost("LoginCustomers")]
        [Produces("application/json", "application/xml", Type = typeof(CustomerDto))]
        public async Task<IActionResult> LoginCustomers(CustomerResource customerResource)
        {

            //CustomerDto
            CustomerResponseData response = new CustomerResponseData();
            var query = new LoginCustomerQuery
            {
                CustomerResource = customerResource
            };
            var customersFromRepo = await _mediator.Send(query);



            if (customersFromRepo.Count > 0)
            {
                //Update OTP
                //var cus = customersFromRepo.FirstOrDefault();
                //UpdateCustomerCommand updateCustomerCommand = new UpdateCustomerCommand();
                //updateCustomerCommand.Id = cus.Id;
                //updateCustomerCommand.OTP = 1010;
                //updateCustomerCommand.CustomerName = cus.CustomerName;
                //updateCustomerCommand.Email = cus.Email;
                //var Updateresponse = await _mediator.Send(updateCustomerCommand);
                //*************************

                //customersFromRepo.FirstOrDefault().OTP = 1234;

                response.status = true;
                response.StatusCode = 1;
                response.message = "Success";
                response.Data = customersFromRepo.FirstOrDefault();
            }
            else
            {
                response.status = false;
                response.StatusCode = 0;
                response.message = "Invalid";
                response.Data = new CustomerDto { };
            }

            return Ok(response);
        }

        /// <summary>
        /// OTP Verify the customer.
        /// </summary>        
        /// <param name="customerResource">The customer resource.</param>
        /// <returns></returns>
        [HttpPost("CustomersOTPVerify")]
        [Produces("application/json", "application/xml", Type = typeof(CustomerDto))]
        public async Task<IActionResult> CustomersOTPVerify(CustomerResource customerResource)
        {
            //CustomerDto
            CustomerResponseData response = new CustomerResponseData();
            var query = new LoginCustomerQuery
            {
                CustomerResource = customerResource
            };
            var customersFromRepo = await _mediator.Send(query);

            if (customersFromRepo.Count > 0)
            {

                if (customersFromRepo.FirstOrDefault().OTP == customerResource.OTP)
                {

                    response.status = true;
                    response.StatusCode = 1;
                    response.message = "Success";
                    response.Data = customersFromRepo.FirstOrDefault();
                }
                else
                {
                    response.status = false;
                    response.StatusCode = 0;
                    response.message = "Invalid OTP";
                    response.Data = new CustomerDto { };
                }
            }
            else
            {
                response.status = false;
                response.StatusCode = 0;
                response.message = "Invalid";
                response.Data = customersFromRepo.FirstOrDefault();
            }

            return Ok(response);

        }


        /// <summary>
        /// Get Non CSD list.
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetNonCSDlist")]
        [Produces("application/json", "application/xml", Type = typeof(List<CounterDto>))]
        public async Task<IActionResult> GetNonCSDlist()
        {
            NonCSDResponseNameData response = new NonCSDResponseNameData();
            var getAllCounterCommand = new GetAllCounterCommand { };
            var result = await _mediator.Send(getAllCounterCommand);

            if (result.Count > 0)
            {
                response.status = true;
                response.StatusCode = 1;
                response.message = "Success";
                response.Data = result;
            }
            else
            {
                response.status = false;
                response.StatusCode = 0;
                response.message = "Invalid";
                response.Data = result;

            }

            return Ok(response);
        }


        /// <summary>
        /// Get All Products List.
        /// </summary>
        /// <param name="productResource"></param>
        /// <returns></returns>
        [HttpGet("GetProductsList")]
        public async Task<IActionResult> GetProductsList(ProductResource productResource)
        {

            ProductListResponseData response = new ProductListResponseData();

            try
            {
                var getAllProductCommand = new GetAllProductCommand
                {
                    ProductResource = productResource
                };
                var result = await _mediator.Send(getAllProductCommand);

                if (result.Count > 0)
                {
                    response.TotalCount = result.TotalCount;
                    response.PageSize = result.PageSize;
                    response.Skip = result.Skip;
                    response.TotalPages = result.TotalPages;

                    response.status = true;
                    response.StatusCode = 1;
                    response.message = "Success";
                    response.Data = result;
                }
                else
                {
                    response.status = false;
                    response.StatusCode = 0;
                    response.message = "Invalid";
                    response.Data = result;

                }

            }
            catch (Exception ex)
            {
                response.status = false;
                response.StatusCode = 0;
                response.message = ex.Message;
            }

            return Ok(response);

        }


        /// <summary>
        /// Get Product Details.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetProductDetails")]
        [Produces("application/json", "application/xml", Type = typeof(ProductDto))]
        public async Task<IActionResult> GetProductDetails(ProductDetailsRequestData productRequestData)
        {
            ProductDetailsResponseData response = new ProductDetailsResponseData();
            if (productRequestData.Id == null || productRequestData.Id == "")
            {
                response.status = false;
                response.StatusCode = 0;
                response.message = "Invalid Product Id";

            }
            else
            {
                try
                {
                    Guid id = new Guid(productRequestData.Id);
                    var getProductCommand = new GetProductCommand { Id = id };
                    var result = await _mediator.Send(getProductCommand);
                    if (result.Success)
                    {
                        response.status = true;
                        response.StatusCode = 1;
                        response.message = "Success";
                        response.Data = result.Data;
                    }
                    else
                    {
                        response.status = false;
                        response.StatusCode = 0;
                        response.message = "Invalid";
                        response.Data = new ProductDto { };
                    }
                }
                catch (Exception ex)
                {
                    response.status = false;
                    response.StatusCode = 0;
                    response.message = ex.Message;
                    
                }
            }
            return Ok(response);

        }


        /// <summary>
        /// Get all Product Categories
        /// </summary>
        /// <param name="getAllProductCategoriesQuery"></param>
        /// <returns></returns>
        //[HttpGet]
        [HttpGet("ProductCategoriesList")]
        [Produces("application/json", "application/xml", Type = typeof(List<ProductCategoryDto>))]
        public async Task<IActionResult> ProductCategoriesList([FromQuery] GetAllProductCategoriesQuery getAllProductCategoriesQuery)
        {
            ProductCategoriesResponseData response = new ProductCategoriesResponseData();
            try
            {
                var result = await _mediator.Send(getAllProductCategoriesQuery);
                if (result.Count > 0)
                {
                    response.status = true;
                    response.StatusCode = 1;
                    response.message = "Success";
                    response.Data = result;
                }
                else
                {
                    response.status = false;
                    response.StatusCode = 0;
                    response.message = "Invalid";
                }
            }
            catch (Exception ex)
            {
                response.status = false;
                response.StatusCode = 0;
                response.message = ex.Message;
            }
            return Ok(response);
        }


       /// <summary>
        /// Create Customer Address.
        /// </summary>
        /// <param name="addCustomerAddressCommand"></param>
        /// <returns></returns>
        [HttpPost("CustomerAddress")]
        [Produces("application/json", "application/xml", Type = typeof(CustomerAddressDto))]
        public async Task<IActionResult> AddCustomerAddress(AddCustomerAddressCommand addCustomerAddressCommand)
        {
            var result = await _mediator.Send(addCustomerAddressCommand);
            if (!result.Success)
            {
                return ReturnFormattedResponse(result);
            }
            //return CreatedAtAction("GetCustomerAddress", new { customerId = response.Data.CustomerId }, response.Data);
            CustomerAddressResponseData response = new CustomerAddressResponseData();
            if (result != null)
            {
                response.status = true;
                response.StatusCode = 1;
                response.message = "Success";
                response.Data = result.Data;
            }
            else
            {
                response.status = false;
                response.StatusCode = 0;
                response.message = "Invalid";
            }
            return Ok(response);
        }

        /// <summary>
        /// Get Customer Address.
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        [HttpGet("CustomerAddress/{customerId}", Name = "GetCustomerAddress")]
        [Produces("application/json", "application/xml", Type = typeof(CustomerAddressDto))]
        public async Task<IActionResult> GetCustomerAddress(Guid customerId)
        {
            var getCustomerAddressCommand = new GetCustomerAddressCommand { CustomerId = customerId };
            var result = await _mediator.Send(getCustomerAddressCommand);
            //return ReturnFormattedResponse(result);

            CustomerAddressResponseData response = new CustomerAddressResponseData();
            if (result != null)
            {
                response.status = true;
                response.StatusCode = 1;
                response.message = "Success";
                response.Data = result.Data;
            }
            else
            {
                response.status = false;
                response.StatusCode = 0;
                response.message = "Invalid";
            }
            return Ok(response);
        }

        /// <summary>
        /// Get Customer Addresses
        /// </summary>
        /// <param name="customerAddressResource"></param>
        /// <returns></returns>

        [HttpGet("GetCustomerAddresses")]
        public async Task<IActionResult> GetCustomerAddresses([FromQuery] CustomerAddressResource customerAddressResource)
        {
            var getCustomerAddressQuery = new GetCustomerAddressQuery
            {
                CustomerAddressResource = customerAddressResource
            };
            var result = await _mediator.Send(getCustomerAddressQuery);

            var paginationMetadata = new
            {
                totalCount = result.TotalCount,
                pageSize = result.PageSize,
                skip = result.Skip,
                totalPages = result.TotalPages
            };
            Response.Headers.Add("X-Pagination",
                Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetadata));

            CustomerAddressListResponseData response = new CustomerAddressListResponseData();
            if (result.Count > 0)
            {
                response.status = true;
                response.StatusCode = 1;
                response.message = "Success";
                response.Data = result;
            }
            else
            {
                response.status = false;
                response.StatusCode = 0;
                response.message = "Invalid";
            }

            return Ok(response);
        }
    }
}
