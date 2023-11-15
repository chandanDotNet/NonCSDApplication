using POS.Data.Dto;
using POS.MediatR.CommandAndQuery;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using POS.API.Helpers;

namespace POS.API.Controllers.CompanyProfile
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CompanyProfileController : BaseController
    {

        public IMediator _mediator { get; set; }

        /// <summary>
        /// Action
        /// </summary>
        /// <param name="mediator"></param>
        public CompanyProfileController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get CompanyProfile
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [Produces("application/json", "application/xml", Type = typeof(CompanyProfileDto))]
        public async Task<IActionResult> GetCompanyProfile()
        {
            var getCompanyProfileQuery = new GetCompanyProfileQuery { };
            var result = await _mediator.Send(getCompanyProfileQuery);
            return Ok(result);
        }
       
        /// <summary>
        /// Update Company Profile
        /// </summary>
        /// <param name="updateCompanyProfileCommand"></param>
        /// <returns></returns>
        [HttpPost]
        [ClaimCheck("SETT_UPDATE_COM_PROFILE")]
        [Produces("application/json", "application/xml", Type = typeof(CompanyProfileDto))]
        public async Task<IActionResult> UpdateCompanyProfile(UpdateCompanyProfileCommand updateCompanyProfileCommand)
        {
            var response = await _mediator.Send(updateCompanyProfileCommand);
            return ReturnFormattedResponse(response);
        }
    }
}
