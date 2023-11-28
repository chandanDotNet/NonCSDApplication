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

        /// <summary>
        /// Logout.
        /// </summary>
        [HttpPost("Logout")]
        public async Task<IActionResult> Logout(string token)
        {
            LogoutResponseData response = new LogoutResponseData();
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

    }
}
