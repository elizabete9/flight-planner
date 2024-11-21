using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Database;
using WebApplication1.Models;
using WebApplication1.Storage;

namespace WebApplication1.Controllers
{
    [Route("admin-api")]
    [ApiController]
    [Authorize]
    public class AdminController(FlightStorage storage) : ControllerBase
    {
        private static readonly object _lockObject = new object();
        private readonly FlightStorage _storage = storage;

        [Route("flights/{id}")]
        [HttpGet]
        public IActionResult GetFlight(int id)
        {
            var flight =_storage.GetFlightById(id);
            if (flight == null)
            {
                return NotFound();
            }
            return Ok(flight);
        }

        [HttpPost]
        [Route("flights")]
        public IActionResult AddFlight(AddFlightRequest flightRequest)
        {
            lock (_lockObject)
            {
                if (flightRequest == null)
                {
                    return BadRequest("Flight request cannot be null.");
                }

                if (flightRequest.From == null ||
                    string.IsNullOrWhiteSpace(flightRequest.From.Country) ||
                    string.IsNullOrWhiteSpace(flightRequest.From.City) ||
                    string.IsNullOrWhiteSpace(flightRequest.From.AirportCode))
                {
                    return BadRequest("Origin airport information is invalid.");
                }

                if (flightRequest.To == null ||
                    string.IsNullOrWhiteSpace(flightRequest.To.Country) ||
                    string.IsNullOrWhiteSpace(flightRequest.To.City) ||
                    string.IsNullOrWhiteSpace(flightRequest.To.AirportCode))
                {
                    return BadRequest("Destination airport information is invalid.");
                }

                if (string.IsNullOrWhiteSpace(flightRequest.Carrier))
                {
                    return BadRequest("Carrier must not be empty.");
                }

                if (!flightRequest.HasValidDates())
                {
                    return BadRequest("Arrival time must be after departure time.");
                }

                if (flightRequest.IsSameAirport())
                {
                    return BadRequest("The origin and destination airports cannot be the same.");
                }

                try
                {
                    var savedFlight = _storage.AddFlight(flightRequest);
                    return Created("", savedFlight);
                }
                catch (InvalidOperationException)
                {
                    return Conflict("Flight already exists.");
                }
            }
        }
       
        [HttpDelete]
        [Route("flights/{id}")]
        public IActionResult DeleteFlight(int id)
        {
            lock (_lockObject)
            {
                var flightDeleted = _storage.DeleteFlight(id); 
                if (!flightDeleted)
                {
                    return Ok(new { message = "Flight not found. Nothing to delete." });
                }

                return Ok("Flight deleted successfully");
            }
        }
            
    }
}
