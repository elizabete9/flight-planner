using AutoMapper;
using FlightPlanner.Core.Models;
using FlightPlanner.Core.Services;
using FlightPlanner.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client.Extensions.Msal;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api")]
    [ApiController]
    public class CustomerController(IEntityService<Flight> flightService,
        IAirportService airportService, IMapper mapper) : ControllerBase
    {
        private readonly IEntityService<Flight> _flightService = flightService;
        private readonly IAirportService _airportService = airportService;
        private readonly IMapper _mapper = mapper;


        [HttpGet]
        [Route("flights/{id}")]
        public IActionResult GetFlightById(int id)
        {
            var flight = _flightService.GetById(id);
            if (flight == null)
            {
                return NotFound();
            }

            return Ok(flight);
        }

        [HttpGet]
        [Route("airports")]
        public IActionResult SearchAirports(string search)
        {
            if (string.IsNullOrWhiteSpace(search))
            {
                return BadRequest("Search term cannot be empty.");
            }
            var results = _airportService.SearchAirports(search);
            return Ok(results);
        }

        [HttpPost]
        [Route("flights/search")]
        public IActionResult FindFlights( SearchFlightsRequest request)
        {
            if (request == null ||
        string.IsNullOrWhiteSpace(request.From) ||
        string.IsNullOrWhiteSpace(request.To) ||
        string.IsNullOrWhiteSpace(request.DepartureDate))
            {
                return BadRequest("Invalid search request.");
            }

            if (string.Equals(request.From, request.To, StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest("Origin and destination cannot be the same.");
            }

            var departureDate = DateTime.Parse(request.DepartureDate);

            var flights = _flightService.List().Where(flight =>
                string.Equals(flight.From.AirportCode, request.From, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(flight.To.AirportCode, request.To, StringComparison.OrdinalIgnoreCase) &&
                DateTime.Parse(flight.DepartureTime) == departureDate.Date);

            var pagedResult = new PageResult<FlightResponse>
            {
                Page = 1,
                TotalItems = flights.Count(),
                Items = flights.Select(f => _mapper.Map<FlightResponse>(f)).ToList()
            };

            return Ok(pagedResult);
        }


    }
}
