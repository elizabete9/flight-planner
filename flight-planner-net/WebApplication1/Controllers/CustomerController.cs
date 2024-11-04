using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.Storage;

namespace WebApplication1.Controllers
{
    [Route("api")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        [HttpGet]
        [Route("airports")]
        public IActionResult SearchAirports(string search)
        {
            var results = AirportService.SearchAirports(search);
            return Ok(results);
        }

        [HttpPost]
        [Route("flights/search")]
        public IActionResult SearchFlights(SearchFlightsRequest searchRequest)
        {
            if (string.Equals(searchRequest.From, searchRequest.To, StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest("The origin and destination airports cannot be the same.");
            }

            var results = FlightStorage.SearchFlights(searchRequest);

            if (results.TotalItems == 0)
            {
                return Ok(new PageResult<Flight> { Page = 0, TotalItems = 0, Items = new List<Flight>() });
            }

            return Ok(results);
        }

        [HttpGet]
        [Route("flights/{id}")]
        public IActionResult GetFlightById(int id)
        {
            var flight = FlightStorage.GetFlightById(id);
            if (flight == null)
            {
                return NotFound();
            }

            return Ok(flight);
        }
    }
}
