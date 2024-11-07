using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.Storage;

namespace WebApplication1.Controllers
{
    [Route("admin-api")]
    [ApiController]
    [Authorize]
    public class AdminController : ControllerBase
    {
        private static readonly object _lockObject = new object();

        [Route("flights/{id}")]
        [HttpGet]
        public IActionResult GetFlight(int id)
        {
            var flight = FlightStorage.GetFlightById(id);
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
                    var flight = FlightStorage.AddFlight(flightRequest);
                    return Created("", flight);
                }
                catch (InvalidOperationException)
                {
                    return Conflict("Flight already exists");
                }
            }
        }

        [HttpDelete]
        [Route("flights/{id}")]
        public IActionResult DeleteFlight(int id)
        {
            lock (_lockObject)
            {
                var flightDeleted = FlightStorage.DeleteFlight(id); 
                if (!flightDeleted)
                {
                    return Ok(new { message = "Flight not found. Nothing to delete." });
                }

                return Ok("Flight deleted successfully");
            }
        }
            
    }
}
