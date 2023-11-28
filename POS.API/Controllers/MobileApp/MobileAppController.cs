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
using POS.MediatR.Country.Commands;
using POS.MediatR.Cart;
using POS.MediatR.PaymentCard.Commands;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using POS.MediatR.Reminder.Commands;
using POS.MediatR.SalesOrder.Commands;
using POS.MediatR.Cart.Commands;
using POS.MediatR.Banner.Command;
using POS.MediatR.Brand.Command;
using System.Security.Claims;
using System.Linq.Dynamic.Core.Tokenizer;
using Newtonsoft.Json.Linq;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

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

        /// <summary>
        /// Login customers.       
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
        [HttpPost("GetProductsList")]
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
        /// <returns></returns>
        [HttpPost("GetProductDetails")]
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
        /// Creates the cart.
        /// </summary>
        /// <param name="addCartCommand">The add cart command.</param>
        /// <returns></returns>
        //[HttpPost, DisableRequestSizeLimit]
        [HttpPost("AddToCart")]
        public async Task<IActionResult> CreateCart([FromBody] AddCartCommand addCartCommand)
        {
            IUDResponseData response = new IUDResponseData();
            var result = await _mediator.Send(addCartCommand);

            if (result.Success)
            {
                response.status = true;
                response.StatusCode = 1;
                response.message = "Success";
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
        /// Updates the cart.
        /// </summary>

        /// <param name="updateCartCommand">The update cart command.</param>
        /// <returns></returns>
        [HttpPut("UpdateToCart")]
        public async Task<IActionResult> UpdateCart([FromBody] UpdateCartCommand updateCartCommand)
        {
            IUDResponseData response = new IUDResponseData();
            //updateCustomerCommand.Id = id;
            var result = await _mediator.Send(updateCartCommand);
            //return ReturnFormattedResponse(response);
            if (result.Success)
            {
                response.status = true;
                response.StatusCode = 1;
                response.message = "Success";
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
        /// Get All Cart List.
        /// </summary>
        /// <param name="cartResource"></param>
        /// <returns></returns>
        [HttpPost("GetCartList")]
        public async Task<IActionResult> GetCartList(CartResource cartResource)
        {

            CartListResponseData response = new CartListResponseData();

            try
            {
                var query = new GetAllCartQuery
                {
                    CartResource = cartResource
                };
                var result = await _mediator.Send(query);

                if (result.Count > 0)
                {
                    response.TotalCount = result.TotalCount;
                    response.PageSize = result.PageSize;
                    response.Skip = result.Skip;
                    response.TotalPages = result.TotalPages;

                    var price = result.Sum(x => x.Total);
                    var discount = result.Sum(x => x.Discount);
                    var items = result.Sum(x => x.Quantity);
                    var deliveryCharges = 0;

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
            }

            return Ok(response);
        }

        /// <summary>
        /// Delete Customer Address.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpDelete("CustomerAddress/{id}")]
        public async Task<IActionResult> DeleteCustomerAddress(Guid Id)
        {
            var deleteCustomerAddressCommand = new DeleteCustomerAddressCommand { Id = Id };
            var result = await _mediator.Send(deleteCustomerAddressCommand);
            //return ReturnFormattedResponse(result);            
            CustomerAddressListResponseData response = new CustomerAddressListResponseData();
            if (result.Success)
            {
                response.status = true;
                response.StatusCode = 1;
                response.message = "Success";
                response.Data = new CustomerAddressDto[0];
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
        /// Delete Cart By Id
        /// </summary>
        /// <param name="deleteCartCommand">The delete cart command.</param>
        /// <returns></returns>
        [HttpDelete("DeleteToCart")]
        public async Task<IActionResult> DeleteCart(DeleteCartCommand deleteCartCommand)
        {
            IUDResponseData response = new IUDResponseData();
            if (deleteCartCommand.Id != null)
            {
                var command = new DeleteCartCommand { Id = deleteCartCommand.Id };
                var result = await _mediator.Send(command);
                if (result.Success)
                {
                    response.status = true;
                    response.StatusCode = 1;
                    response.message = "Success";
                }
                else
                {
                    response.status = false;
                    response.StatusCode = 0;
                    response.message = "Invalid";

                }
            }
            else
            {
                response.status = false;
                response.StatusCode = 0;
                response.message = "Invalid Cart Id";
            }

            return Ok(response);
        }

        /// <summary>
        /// Update Customer Address.
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="updateCustomerAddressCommand"></param>
        /// <returns></returns>
        [HttpPut("CustomerAddress/{Id}")]
        [Produces("application/json", "application/xml", Type = typeof(CustomerAddressDto))]
        public async Task<IActionResult> UpdateCustomerAddress(Guid Id, UpdateCustomerAddressCommand updateCustomerAddressCommand)
        {
            updateCustomerAddressCommand.Id = Id;
            var result = await _mediator.Send(updateCustomerAddressCommand);
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

        //Wishlist-list

        /// <summary>
        /// Creates the cart.
        /// </summary>
        /// <param name="addWishlistCommand">The add cart command.</param>
        /// <returns></returns>
        //[HttpPost, DisableRequestSizeLimit]
        [HttpPost("AddToWishlist")]
        public async Task<IActionResult> CreateWishlist([FromBody] AddWishlistCommand addWishlistCommand)
        {
            IUDResponseData response = new IUDResponseData();
            var result = await _mediator.Send(addWishlistCommand);

            if (result.Success)
            {
                response.status = true;
                response.StatusCode = 1;
                response.message = "Success";
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
        /// Get All Cart List.
        /// </summary>
        /// <param name="wishlistResource"></param>
        /// <returns></returns>
        [HttpPost("GetWishlist")]
        public async Task<IActionResult> GetWishlist(WishlistResource wishlistResource)
        {

            WishlistResponseData response = new WishlistResponseData();

            try
            {
                var query = new GetAllWishlistQuery
                {
                    WishlistResource = wishlistResource
                };
                var result = await _mediator.Send(query);

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
        /// Delete Wishlist By Id
        /// </summary>
        /// <param name="deleteWishlistCommand">The delete cart command.</param>
        /// <returns></returns>
        [HttpDelete("DeleteWishlist")]
        public async Task<IActionResult> DeleteWishlist(DeleteWishlistCommand deleteWishlistCommand)
        {
            IUDResponseData response = new IUDResponseData();
            if (deleteWishlistCommand.Id != null)
            {
                var command = new DeleteWishlistCommand { Id = deleteWishlistCommand.Id };
                var result = await _mediator.Send(command);
                if (result.Success)
                {
                    response.status = true;
                    response.StatusCode = 1;
                    response.message = "Success";
                }
                else
                {
                    response.status = false;
                    response.StatusCode = 0;
                    response.message = "Invalid";

                }
            }
            else
            {
                response.status = false;
                response.StatusCode = 0;
                response.message = "Invalid Cart Id";
            }

            return Ok(response);
        }

        /// <summary>
        /// Add Payment Card.
        /// </summary>
        /// <param name="addPaymentCardCommand"></param>
        /// <returns></returns>
        [HttpPost("PaymentCard")]
        [Produces("application/json", "application/xml", Type = typeof(PaymentCardDto))]
        public async Task<IActionResult> AddPaymentCard(AddPaymentCardCommand addPaymentCardCommand)
        {
            var result = await _mediator.Send(addPaymentCardCommand);
            if (!result.Success)
            {
                return ReturnFormattedResponse(result);
            }
            //return CreatedAtAction("GetCustomerAddress", new { customerId = response.Data.CustomerId }, response.Data);
            PaymentCardResponseData response = new PaymentCardResponseData();

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
        /// Get Payment Cards
        /// </summary>
        /// <param name="paymentCardResource"></param>
        /// <returns></returns>

        [HttpGet("GetPaymentCards")]
        public async Task<IActionResult> GetPaymentCards([FromQuery] PaymentCardResource paymentCardResource)
        {
            var getPaymentCardQuery = new GetPaymentCardQuery
            {
                PaymentCardResource = paymentCardResource
            };
            var result = await _mediator.Send(getPaymentCardQuery);

            var paginationMetadata = new
            {
                totalCount = result.TotalCount,
                pageSize = result.PageSize,
                skip = result.Skip,
                totalPages = result.TotalPages
            };
            Response.Headers.Add("X-Pagination",
                Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetadata));

            PaymentCardListResponseData response = new PaymentCardListResponseData();
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
            }

            return Ok(response);
        }

        /// <summary>
        /// Delete CPayment Card.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpDelete("PaymentCard/{id}")]
        public async Task<IActionResult> DeletePaymentCard(Guid Id)
        {
            var deletePaymentCardCommand = new DeletePaymentCardCommand { Id = Id };
            var result = await _mediator.Send(deletePaymentCardCommand);
            //return ReturnFormattedResponse(result);            
            PaymentCardListResponseData response = new PaymentCardListResponseData();
            if (result.Success)
            {
                response.status = true;
                response.StatusCode = 1;
                response.message = "Success";
                response.Data = new PaymentCardDto[0];
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
        /// Update Payment Card.
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="updatePaymentCardCommand"></param>
        /// <returns></returns>
        [HttpPut("PaymentCard/{Id}")]
        [Produces("application/json", "application/xml", Type = typeof(PaymentCardDto))]
        public async Task<IActionResult> UpdatePaymentCard(Guid Id, UpdatePaymentCardCommand updatePaymentCardCommand)
        {
            updatePaymentCardCommand.Id = Id;
            var result = await _mediator.Send(updatePaymentCardCommand);
            //return ReturnFormattedResponse(result);           

            PaymentCardResponseData response = new PaymentCardResponseData();

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
        /// Gets the customer profile.
        /// </summary>
        /// <param name="customerQuery">The identifier.</param>
        /// <returns></returns>
        [HttpPost("GetCustomerProfile")]
        public async Task<IActionResult> GetCustomerProfile(GetCustomerQuery customerQuery)
        {
            CustomerProfileResponseData response = new CustomerProfileResponseData();
            var query = new GetCustomerQuery { Id = customerQuery.Id };
            var result = await _mediator.Send(query);

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
        /// Updates the customer Profile.
        /// </summary>       
        /// <param name="updateCustomerCommand">The update customer Profile command.</param>
        /// <returns></returns>
        [HttpPut("UpdateCustomerProfile"), DisableRequestSizeLimit]
        public async Task<IActionResult> UpdateCustomerProfile(UpdateCustomerCommand updateCustomerCommand)
        {
            IUDResponseData response = new IUDResponseData();
            // updateCustomerCommand.Id = id;
            var result = await _mediator.Send(updateCustomerCommand);
            if (result.Success)
            {
                response.status = true;
                response.StatusCode = 1;
                response.message = "Success";
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
        /// Gets all customer's order list.
        /// </summary>
        /// <param name="salesOrderResource">The update customer Profile command.</param>
        /// <returns></returns>
        [HttpPost("GetAllCustomerOrderList")]
        [Produces("application/json", "application/xml", Type = typeof(List<SalesOrderDto>))]
        public async Task<IActionResult> GetAllCustomerOrderList(SalesOrderResource salesOrderResource)
        {
            CustomerOrderListResponseData response = new CustomerOrderListResponseData();
            var getAllSalesOrderQuery = new GetAllSalesOrderCommand
            {
                SalesOrderResource = salesOrderResource
            };
            var result = await _mediator.Send(getAllSalesOrderQuery);

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

            return Ok(response);
        }


        /// <summary>
        /// Get Sales Order Details.
        /// </summary>
        /// <param name="salesOrderCommand"></param>
        /// <returns></returns>
        [HttpPost("GetCustomerOrderDetails")]
        [Produces("application/json", "application/xml", Type = typeof(List<SalesOrderDto>))]
        public async Task<IActionResult> GetCustomerOrderDetails(GetSalesOrderCommand salesOrderCommand)
        {
            CustomerOrderDetailsResponseData response = new CustomerOrderDetailsResponseData();
            var getSalesOrderQuery = new GetSalesOrderCommand
            {
                Id = salesOrderCommand.Id
            };
            var result = await _mediator.Send(getSalesOrderQuery);

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
                response.message = "Invalid - " + result.Errors.First();
                response.Data = new SalesOrderDto { };
            }

            return Ok(response);
        }


        /// <summary>
        /// Creates the Customer sales order.
        /// </summary>
        /// <param name="addCustomerSalesOrderCommand">The add sales order command.</param>
        /// <returns></returns>
        [HttpPost("CreateCustomerSalesOrder")]
        [Produces("application/json", "application/xml", Type = typeof(SalesOrderDto))]
        public async Task<IActionResult> CreateCustomerSalesOrder(AddSalesOrderCommand addCustomerSalesOrderCommand)
        {
            IUDResponseData response = new IUDResponseData();
            var result = await _mediator.Send(addCustomerSalesOrderCommand);
            if (result.Success)
            {
                var command = new DeleteCartByCustomerCommand { CustomerId = addCustomerSalesOrderCommand.CustomerId };
                var result2 = await _mediator.Send(command);


                response.status = true;
                response.StatusCode = 1;
                response.message = "Success";
            }
            else
            {
                response.status = false;
                response.StatusCode = 0;
                response.message = "Invalid - " + result.Errors.First();
            }
            return Ok(response);
        }

        /// <summary>
        /// Get Customer Notifications.
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetCustomerNotifications")]
        public async Task<IActionResult> GetCustomerNotifications([FromQuery] ReminderResource reminderResource)
        {
            //var getUserNotificationCountQuery = new GetUserNotificationCountQuery { };
            //var result = await _mediator.Send(getUserNotificationCountQuery);
            //return Ok(result);
            var getReminderNotificationQuery = new GetReminderNotificationQuery
            {
                ReminderResource = reminderResource
            };
            var result = await _mediator.Send(getReminderNotificationQuery);

            var paginationMetadata = new
            {
                totalCount = result.TotalCount,
                pageSize = result.PageSize,
                skip = result.Skip,
                totalPages = result.TotalPages
            };
            Response.Headers.Add("X-Pagination",
                Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetadata));

            ReminderListResponseData response = new ReminderListResponseData();
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
            }

            return Ok(response);

        }


        /// <summary>
        /// Update the Customer Sales order return.
        /// </summary>
        /// <param name="UpdateSalesOrderReturnCommand">The update customer Sales order command.</param>
        /// <returns></returns>
        [HttpPut("CustomerSalesOrderReturn")]
        [Produces("application/json", "application/xml")]
        public async Task<IActionResult> UpdateCustomerSalesOrderReturn(UpdateSalesOrderReturnCommand updateSalesOrderReturnCommand)
        {
            IUDResponseData response = new IUDResponseData();
            var result = await _mediator.Send(updateSalesOrderReturnCommand);
            if (result.Success)
            {
                response.status = true;
                response.StatusCode = 1;
                response.message = "Success";
            }
            else
            {
                response.status = false;
                response.StatusCode = 0;
                response.message = "Invalid - " + result.Errors.First();
            }
            return Ok(response);
        }

        /// <summary>
        /// Get Order Summary
        /// </summary>
        /// <param name="cartResource"></param>
        /// <returns></returns>
        [HttpPost("GetOrderSummary")]
        public async Task<IActionResult> GetOrderSummary(CartResource cartResource)
        {

            CustomerOrderSummaryResponseData response = new CustomerOrderSummaryResponseData();

            try
            {
                var query = new GetAllCartQuery
                {
                    CartResource = cartResource
                };
                var result = await _mediator.Send(query);

                if (result.Count > 0)
                {

                    var Data = new OrderSummary
                    {
                        Price = result.Sum(x => x.Total),
                        Discount = result.Sum(x => x.Discount),
                        DeliveryCharges = 0,
                        Items = result.Sum(x => x.Quantity),
                    };

                    response.status = true;
                    response.StatusCode = 1;
                    response.message = "Success";
                    response.Data = Data;
                }
                else
                {
                    response.status = false;
                    response.StatusCode = 0;
                    response.message = "Invalid";
                    response.Data = new OrderSummary { };
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

        [Authorize]
        /// <summary>
        /// Logout.
        /// </summary>
        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            LogoutResponseData response = new LogoutResponseData();
            string token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJEQl9TVEFUSVNUSUNTIjoidHJ1ZSIsIkRCX0JFU1RfU0VMTElOR19QUk9TIjoidHJ1ZSIsIkRCX1JFTUlOREVSUyI6InRydWUiLCJEQl9MQVRFU1RfSU5RVUlSSUVTIjoidHJ1ZSIsIkRCX1JFQ0VOVF9TT19TSElQTUVOVCI6InRydWUiLCJEQl9SRUNFTlRfUE9fREVMSVZFUlkiOiJ0cnVlIiwiUFJPX1ZJRVdfUFJPRFVDVFMiOiJ0cnVlIiwiUFJPX0FERF9QUk9EVUNUIjoidHJ1ZSIsIlBST19VUERBVEVfUFJPRFVDVCI6InRydWUiLCJQUk9fREVMRVRFX1BST0RVQ1QiOiJ0cnVlIiwiUFJPX01BTkFHRV9QUk9fQ0FUIjoidHJ1ZSIsIlBST19NQU5BR0VfVEFYIjoidHJ1ZSIsIlBST19NQU5BR0VfVU5JVCI6InRydWUiLCJQUk9fTUFOQUdFX0JSQU5EIjoidHJ1ZSIsIlBST19NQU5BR0VfV0FSRUhPVVNFIjoidHJ1ZSIsIlNVUFBfVklFV19TVVBQTElFUlMiOiJ0cnVlIiwiU1VQUF9BRERfU1VQUExJRVIiOiJ0cnVlIiwiU1VQUF9VUERBVEVfU1VQUExJRVIiOiJ0cnVlIiwiU1VQUF9ERUxFVEVfU1VQUExJRVIiOiJ0cnVlIiwiQ1VTVF9WSUVXX0NVU1RPTUVSUyI6InRydWUiLCJDVVNUX0FERF9DVVNUT01FUiI6InRydWUiLCJDVVNUX1VQREFURV9DVVNUT01FUiI6InRydWUiLCJDVVNUX0RFTEVURV9DVVNUT01FUiI6InRydWUiLCJJTlFfVklFV19JTlFVSVJJRVMiOiJ0cnVlIiwiSU5RX0FERF9JTlFVSVJZIjoidHJ1ZSIsIklOUV9VUERBVEVfSU5RVUlSWSI6InRydWUiLCJJTlFfREVMRVRFX0lOUVVJUlkiOiJ0cnVlIiwiSU5RX01BTkFHRV9JTlFfU1RBVFVTIjoidHJ1ZSIsIklOUV9NQU5BR0VfSU5RX1NPVVJDRSI6InRydWUiLCJJTlFfTUFOQUdFX1JFTUlOREVSUyI6InRydWUiLCJQT1JfVklFV19QT19SRVFVRVNUUyI6InRydWUiLCJQT1JfQUREX1BPX1JFUVVFU1QiOiJ0cnVlIiwiUE9SX1VQREFURV9QT19SRVFVRVNUIjoidHJ1ZSIsIlBPUl9ERUxFVEVfUE9fUkVRVUVTVCI6InRydWUiLCJQT1JfQ09OVkVSVF9UT19QTyI6InRydWUiLCJQT1JfR0VORVJBVEVfSU5WT0lDRSI6InRydWUiLCJQT1JfUE9SX0RFVEFJTCI6InRydWUiLCJQT19WSUVXX1BVUkNIQVNFX09SREVSUyI6InRydWUiLCJQT19BRERfUE8iOiJ0cnVlIiwiUE9fVVBEQVRFX1BPIjoidHJ1ZSIsIlBPX0RFTEVURV9QTyI6InRydWUiLCJQT19WSUVXX1BPX0RFVEFJTCI6InRydWUiLCJQT19SRVRVUk5fUE8iOiJ0cnVlIiwiUE9fVklFV19QT19QQVlNRU5UUyI6InRydWUiLCJQT19BRERfUE9fUEFZTUVOVCI6InRydWUiLCJQT19ERUxFVEVfUE9fUEFZTUVOVCI6InRydWUiLCJQT19HRU5FUkFURV9JTlZPSUNFIjoidHJ1ZSIsIlNPX1ZJRVdfU0FMRVNfT1JERVJTIjoidHJ1ZSIsIlNPX0FERF9TTyI6InRydWUiLCJTT19VUERBVEVfU08iOiJ0cnVlIiwiU09fREVMRVRFX1NPIjoidHJ1ZSIsIlNPX1ZJRVdfU09fREVUQUlMIjoidHJ1ZSIsIlNPX1JFVFVSTl9TTyI6InRydWUiLCJTT19WSUVXX1NPX1BBWU1FTlRTIjoidHJ1ZSIsIlNPX0FERF9TT19QQVlNRU5UIjoidHJ1ZSIsIlNPX0RFTEVURV9TT19QQVlNRU5UIjoidHJ1ZSIsIlNPX0dFTkVSQVRFX0lOVk9JQ0UiOiJ0cnVlIiwiSU5WRV9WSUVXX0lOVkVOVE9SSUVTIjoidHJ1ZSIsIklOVkVfTUFOQUdFX0lOVkVOVE9SWSI6InRydWUiLCJFWFBfVklFV19FWFBFTlNFUyI6InRydWUiLCJFWFBfQUREX0VYUEVOU0UiOiJ0cnVlIiwiRVhQX1VQREFURV9FWFBFTlNFIjoidHJ1ZSIsIkVYUF9ERUxFVEVfRVhQRU5TRSI6InRydWUiLCJFWFBfTUFOQUdFX0VYUF9DQVRFR09SWSI6InRydWUiLCJSRVBfUE9fUkVQIjoidHJ1ZSIsIlJFUF9TT19SRVAiOiJ0cnVlIiwiUkVQX1BPX1BBWU1FTlRfUkVQIjoidHJ1ZSIsIlJFUF9FWFBFTlNFX1JFUCI6InRydWUiLCJSRVBfU09fUEFZTUVOVF9SRVAiOiJ0cnVlIiwiUkVQX1NBTEVTX1ZTX1BVUkNIQVNFX1JFUCI6InRydWUiLCJSRU1fVklFV19SRU1JTkRFUlMiOiJ0cnVlIiwiUkVNX0FERF9SRU1JTkRFUiI6InRydWUiLCJSRU1fVVBEQVRFX1JFTUlOREVSIjoidHJ1ZSIsIlJFTV9ERUxFVEVfUkVNSU5ERVIiOiJ0cnVlIiwiUk9MRVNfVklFV19ST0xFUyI6InRydWUiLCJST0xFU19BRERfUk9MRSI6InRydWUiLCJST0xFU19VUERBVEVfUk9MRSI6InRydWUiLCJST0xFU19ERUxFVEVfUk9MRSI6InRydWUiLCJVU1JfVklFV19VU0VSUyI6InRydWUiLCJVU1JfQUREX1VTRVIiOiJ0cnVlIiwiVVNSX1VQREFURV9VU0VSIjoidHJ1ZSIsIlVTUl9ERUxFVEVfVVNFUiI6InRydWUiLCJVU1JfUkVTRVRfUFdEIjoidHJ1ZSIsIlVTUl9BU1NJR05fVVNSX1JPTEVTIjoidHJ1ZSIsIlVTUl9BU1NJR05fVVNSX1BFUk1JU1NJT05TIjoidHJ1ZSIsIlVTUl9PTkxJTkVfVVNFUlMiOiJ0cnVlIiwiRU1BSUxfTUFOQUdFX0VNQUlMX1NNVFBfU0VUVElOUyI6InRydWUiLCJFTUFJTF9NQU5BR0VfRU1BSUxfVEVNUExBVEVTIjoidHJ1ZSIsIkVNQUlMX1NFTkRfRU1BSUwiOiJ0cnVlIiwiU0VUVF9VUERBVEVfQ09NX1BST0ZJTEUiOiJ0cnVlIiwiU0VUVF9NQU5BR0VfQ09VTlRSWSI6InRydWUiLCJTRVRUX01BTkFHRV9DSVRZIjoidHJ1ZSIsIkxPR1NfVklFV19MT0dJTl9BVURJVFMiOiJ0cnVlIiwiTE9HU19WSUVXX0VSUk9SX0xPR1MiOiJ0cnVlIiwiUkVQX1BST19QUF9SRVAiOiJ0cnVlIiwiUkVQX0NVU1RfUEFZTUVOVF9SRVAiOiJ0cnVlIiwiUkVQX1BST19TT19SRVBPUlQiOiJ0cnVlIiwiUkVQX1NVUF9QQVlNRU5UX1JFUCI6InRydWUiLCJSRVBfU1RPQ0tfUkVQT1JUIjoidHJ1ZSIsIlBPU19QT1MiOiJ0cnVlIiwiUkVQX1ZJRVdfV0FSX1JFUCI6InRydWUiLCJSRVBfVklFV19QUk9fTE9TU19SRVAiOiJ0cnVlIiwic3ViIjoiNGIzNTJiMzctMzMyYS00MGM2LWFiMDUtZTM4ZmNmMTA5NzE5IiwiRW1haWwiOiJhZG1pbkBnbWFpbC5jb20iLCJuYmYiOjE3MDEwODA0MjgsImV4cCI6MTcwMTEyMzYyOCwiaXNzIjoiaHR0cDovL2xvY2FsaG9zdDo1MDAwIiwiYXVkIjoiUFRDVXNlcnMifQ.bZvhsh1KWB8xrO7JZh0Mral3RO0pdoevQamJyZJj9Yg";
           
            var principal = GetPrincipalFromExpiredToken(token);
            var expClaim = principal.Claims.First(x => x.Type == "Email").Value;
            var identity = principal.Identity as ClaimsIdentity;
            var tok = identity.FindFirst("Token");
            identity.RemoveClaim(identity.FindFirst("Token"));
            //var existingClaim = identity.FindFirst(key);
            response.status = true;
            response.StatusCode = 1;
            response.message = "Success";
            response.Data = "Logout Successfully";
            return Ok(response);
        }


        /// <summary>
        /// Gets the new Sales order number.
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetNewOrderNumber")]
        public async Task<IActionResult> GetNewSalesOrderNumber()
        {
            OrderNumberResponseData response = new OrderNumberResponseData();
            var getNewSalesOrderNumberQuery = new GetNewSalesOrderNumberCommand { };
            var result = await _mediator.Send(getNewSalesOrderNumberQuery);
            if (result != null)
            {
                response.status = true;
                response.StatusCode = 1;
                response.message = "Success";
                response.OrderNumber = result;
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
        /// Send mail.
        /// </summary>
        /// <param name="sendEmailCommand"></param>
        /// <returns></returns>
        [HttpPost("SendAllEmail")]        
        [Produces("application/json", "application/xml", Type = typeof(void))]
        public async Task<IActionResult> SendAllEmail(SendEmailCommand sendEmailCommand)
        {
            SendEmailResponseData response = new SendEmailResponseData();
            var result = await _mediator.Send(sendEmailCommand);
            if (result.Success)
            {
                response.status = true;
                response.StatusCode = 1;
                response.message = "Success";                
            }
            else
            {
                response.status = false;
                response.StatusCode = 0;
                response.message = "Invalid - " + result.Errors.First();
            }
            return Ok(response);

        }

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {            
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false, //you might want to validate the audience and issuer depending on your use case
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("This*Is&A!Long)Key(For%Creating@A$SymmetricKey")),
                //ValidateLifetime = false, //here we are saying that we don't care about the token's expiration date               
                ClockSkew = TimeSpan.Zero
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;
        }

        /// <summary>
        /// Add Banner.
        /// </summary>
        /// <param name="addBannerCommand"></param>
        /// <returns></returns>
        [HttpPost("Banner")]
        [Produces("application/json", "application/xml", Type = typeof(BannerDto))]
        public async Task<IActionResult> AddBanner(AddBannerCommand addBannerCommand)
        {
            var response = await _mediator.Send(addBannerCommand);
            if (!response.Success)
            {
                return ReturnFormattedResponse(response);
            }
            return CreatedAtAction("GetBanners", new { id = response.Data.Id }, response.Data);
        }

        /// <summary>
        /// Get Banners.
        /// </summary>
        /// <returns></returns>
        [HttpGet("Banners")]
        [Produces("application/json", "application/xml", Type = typeof(List<BannerDto>))]
        public async Task<IActionResult> GetBanners()
        {
            BannerListResponseData response = new BannerListResponseData();
            var getAllBannerCommand = new GetAllBannerCommand { };
            var result = await _mediator.Send(getAllBannerCommand);

            if (result.Count > 0)
            {
                //response.TotalCount = result.TotalCount;
                //response.PageSize = result.PageSize;
                //response.Skip = result.Skip;
                //response.TotalPages = result.TotalPages;

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
        /// Add Login Page Banner.
        /// </summary>
        /// <param name="addLoginPageBannerCommand"></param>
        /// <returns></returns>
        [HttpPost("LoginPageBanner")]
        [Produces("application/json", "application/xml", Type = typeof(LoginPageBannerDto))]
        public async Task<IActionResult> AddLoginPageBanner(AddLoginPageBannerCommand addLoginPageBannerCommand)
        {
            var response = await _mediator.Send(addLoginPageBannerCommand);
            if (!response.Success)
            {
                return ReturnFormattedResponse(response);
            }
            return CreatedAtAction("GetLoginPageBanners", new { id = response.Data.Id }, response.Data);
        }

        /// <summary>
        /// Get Login Page Banners.
        /// </summary>
        /// <returns></returns>
        [HttpGet("LoginPageBanners")]
        [Produces("application/json", "application/xml", Type = typeof(List<LoginPageBannerDto>))]
        public async Task<IActionResult> GetLoginPageBanners()
        {
            LoginPageBannerListResponseData response = new LoginPageBannerListResponseData();
            var getAllLoginPageBannerCommand = new GetAllLoginPageBannerCommand { };
            var result = await _mediator.Send(getAllLoginPageBannerCommand);

            if (result.Count > 0)
            {
                //response.TotalCount = result.TotalCount;
                //response.PageSize = result.PageSize;
                //response.Skip = result.Skip;
                //response.TotalPages = result.TotalPages;

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
    }
}
