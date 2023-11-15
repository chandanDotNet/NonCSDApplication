using POS.MediatR.CommandAndQuery;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using POS.Data.Resources;
using POS.MediatR.City.Command;
using POS.MediatR.City.Commands;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using POS.API.Helpers;

namespace POS.API.Controllers.City
{
    [Route("api/[controller]")]
    [ApiController]
    public class CityController : BaseController
    {
        readonly IMediator _mediator;

        public CityController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Gets the cities.
        /// </summary>
        /// <param name="countryName">Name of the country.</param>
        /// <param name="cityName">Name of the city.</param>
        /// <returns></returns>
        [HttpGet("country")]
        [ClaimCheck("SETT_MANAGE_CITY")]
        public async Task<IActionResult> GetCitiesByName(string countryName, string cityName)
        {
            var query = new GetCitiesByContryNameQuery
            {
                CityName = cityName,
                CountryName = countryName
            };

            var result = await _mediator.Send(query);
            return ReturnFormattedResponse(result);
        }

        /// <summary>
        /// Get All Cities
        /// </summary>
        /// <param name="cityResource"></param>
        /// <returns></returns>

        [HttpGet(Name = "GetCities")]
        [ClaimCheck("SETT_MANAGE_CITY")]
        public async Task<IActionResult> GetCities([FromQuery] CityResource cityResource)
        {
            var getAllCityQuery = new GetAllCityQuery
            {
                CityResource = cityResource
            };
            var result = await _mediator.Send(getAllCityQuery);

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
        /// Add City
        /// </summary>
        /// <param name="addCityCommand"></param>
        /// <returns></returns>
        [HttpPost]
        [ClaimCheck("SETT_MANAGE_CITY")]
        public async Task<IActionResult> AddCity([FromBody] AddCityCommand addCityCommand)
        {
            var result = await _mediator.Send(addCityCommand);
            return ReturnFormattedResponse(result);
        }
        /// <summary>
        /// Update City By Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updateCityCommand"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ClaimCheck("SETT_MANAGE_CITY")]
        public async Task<IActionResult> UpdateCity(Guid id, [FromBody] UpdateCityCommand updateCityCommand)
        {
            updateCityCommand.Id = id;
            var result = await _mediator.Send(updateCityCommand);
            return ReturnFormattedResponse(result);
        }
        /// <summary>
        /// Delete City By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ClaimCheck("SETT_MANAGE_CITY")]
        public async Task<IActionResult> DeleteCity(Guid id)
        {
            var deleteCityCommand = new DeleteCityCommand { Id = id };
            var result = await _mediator.Send(deleteCityCommand);
            return ReturnFormattedResponse(result);
        }
    }
}
