using POS.Data;
using POS.Helper;
using POS.MediatR.CommandAndQuery;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using POS.API.Helpers;
using POS.MediatR.Customer.Commands;
using POS.MediatR.Commands;

namespace POS.API.Controllers.Customer
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CustomerController : BaseController
    {
        private readonly IMediator _mediator;

        public CustomerController(IMediator mediator)
        {
            _mediator = mediator;
        }

        //
        /// <summary>
        /// Gets the customers.
        /// 
        /// </summary>
        /// <param name="customerResource">The customer resource.</param>
        /// <returns></returns>
        [HttpGet(Name = "GetCustomers")]
        [ClaimCheck("CUST_VIEW_CUSTOMERS")]
        public async Task<IActionResult> GetCustomers([FromQuery] CustomerResource customerResource)
        {
            var query = new GetAllCustomerQuery
            {
                CustomerResource = customerResource
            };
            var customersFromRepo = await _mediator.Send(query);
            var paginationMetadata = new
            {
                totalCount = customersFromRepo.TotalCount,
                pageSize = customersFromRepo.PageSize,
                totalPages = customersFromRepo.TotalPages,
                skip = customersFromRepo.Skip,
            };

            Response.Headers.Add("X-Pagination",
                Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetadata));

            return Ok(customersFromRepo);
        }

        [HttpGet("search",Name = "GetCustomerSearch")]
        [ClaimCheck("CUST_VIEW_CUSTOMERS")]
        public async Task<IActionResult> GetCustomersBySearch([FromQuery] SearchCustomerQuery searchCustomerQuery)
        {
            var customersFromRepo = await _mediator.Send(searchCustomerQuery);
            return Ok(customersFromRepo);
        }

        /// <summary>
        /// Gets the customer.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "GetCustomer")]
        [ClaimCheck("CUST_VIEW_CUSTOMERS")]
        public async Task<IActionResult> GetCustomer(Guid id)
        {
            var query = new GetCustomerQuery { Id = id };
            var response = await _mediator.Send(query);
            return ReturnFormattedResponse(response);
        }

        /// <summary>
        /// Creates the customer.
        /// </summary>
        /// <param name="addCustomerCommand">The add customer command.</param>
        /// <returns></returns>
        [HttpPost, DisableRequestSizeLimit]
        [ClaimCheck("CUST_ADD_CUSTOMER")]
        public async Task<IActionResult> CreateCustomer([FromBody] AddCustomerCommand addCustomerCommand)
        {
            var response = await _mediator.Send(addCustomerCommand);
            return ReturnFormattedResponse(response);
        }


        /// <summary>
        /// Updates the customer.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="updateCustomerCommand">The update customer command.</param>
        /// <returns></returns>
        [HttpPut("{id}"), DisableRequestSizeLimit]
        [ClaimCheck("CUST_UPDATE_CUSTOMER")]
        public async Task<IActionResult> UpdateCustomer(Guid id, [FromBody] UpdateCustomerCommand updateCustomerCommand)
        {
            updateCustomerCommand.Id = id;
            var response = await _mediator.Send(updateCustomerCommand);
            return ReturnFormattedResponse(response);
        }

        /// <summary>
        /// Delete Customer By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ClaimCheck("CUST_DELETE_CUSTOMER")]
        public async Task<IActionResult> DeleteCustomer(Guid id)
        {
            var command = new DeleteCustomerCommand { Id = id };
            var result = await _mediator.Send(command);
            return ReturnFormattedResponse(result);
        }

        /// <summary>
        /// Check Email for Phone Exists.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="email"></param>
        /// <param name="mobileNo"></param>
        /// <returns></returns>
        [HttpGet("{id}/Exist")]
        public async Task<IActionResult> EmailOrPhoneExist(Guid id, string email, string mobileNo)
        {
            var command = new EmailOrPhoneExistCheckQuery
            {
                Email = email,
                MobileNo = mobileNo,
                Id = id
            };
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        /// <summary>
        /// Get Customer Payment Report
        /// </summary>
        /// <param name="customerResource"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetCustomerPayment")]
        //[ClaimCheck("SUPP_VIEW_SUPPLIERS")]
        public async Task<IActionResult> GetCustomerPayment([FromQuery] CustomerResource customerResource)
        {
            var result = await _mediator.Send(new GetCustomerPaymentsQuery
            {
                CustomerResource = customerResource
            });

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
        /// Get Customer Payment Report
        /// </summary>
        /// <param name="customerResource"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetCustomerPayment/report")]
        //[ClaimCheck("SUPP_VIEW_SUPPLIERS")]
        public async Task<IActionResult> GetCustomerPaymentReport([FromQuery] CustomerResource customerResource)
        {
            var result = await _mediator.Send(new GetCustomerPaymentsReportQuery
            {
                CustomerResource = customerResource
            });

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
        /// Get Walk In Customer
        /// </summary>
        /// <returns></returns>

        [HttpGet("walkIn")]
        public async Task<IActionResult> GetWalkInCustomer()
        {
            var command = new GetWalkInCusomerCommand{};
            var response = await _mediator.Send(command);
            return Ok(response.Data);
        }
        
    }
}
