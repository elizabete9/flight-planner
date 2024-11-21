using Microsoft.AspNetCore.Mvc;
using WebApplication1.Storage;

namespace WebApplication1.Controllers
{
    [Route("testing-api")]
    [ApiController]
    public class TestingController(FlightStorage storage) : ControllerBase
    {
        private readonly FlightStorage _storage = storage;

        [HttpPost]
        [Route("clear")]
        public IActionResult Clear() 
        {
            _storage.ClearFlights();

            return Ok();
        }
    }
}
