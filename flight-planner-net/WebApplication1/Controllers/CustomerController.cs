using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.Storage;

namespace WebApplication1.Controllers
{
    [Route("api")]
    [ApiController]
    public class CustomerController(FlightStorage storage, AirportService airport) : ControllerBase
    {
        private readonly FlightStorage _storage = storage;
        private readonly AirportService _airport = airport;

        [HttpGet]
        [Route("airports")]
        public IActionResult SearchAirports(string search)
        {
            if (string.IsNullOrWhiteSpace(search))
            {
                return BadRequest("Search term cannot be empty.");
            }
            var results = _airport.SearchAirports(search);
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

            var results = _storage.SearchFlights(searchRequest);

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
            var flight = _storage.GetFlightById(id);
            if (flight == null)
            {
                return NotFound();
            }

            return Ok(flight);
        }
    }
}
