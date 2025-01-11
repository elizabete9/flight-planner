using AutoMapper;
using Azure.Core;
using FlightPlanner.Core.Models;
using FlightPlanner.Core.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client.Extensions.Msal;
using WebApplication1.Models;
using WebApplication1.Validations;
using IValidator = WebApplication1.Validations.IValidator;

namespace WebApplication1.Controllers
{
    [Route("admin-api")]
    [ApiController]
    [Authorize]
    public class AdminController(IFlightService flightService, 
        IEnumerable<IValidator> validators, 
        IValidator<Flight> validator,
        IMapper mapper) : ControllerBase
    {
        private static readonly object _lockObject = new object();
        private readonly IFlightService _flightService = flightService;
        private readonly IEnumerable<IValidator> _validators = validators;
        private readonly IValidator<Flight> _validator = validator;
        private readonly IMapper _mapper = mapper;

        [Route("flights/{id}")]
        [HttpGet]
        public IActionResult GetFlight(int id)
        {
            var result = _flightService.GetFullFlightById(id);
            if (result == null) 
            {
                return NotFound();
            }
            var response = _mapper.Map<FlightResponse>(result);
            return Ok(result);
        }

        [HttpPost]
        [Route("flights")]
        public IActionResult AddFlight(AddFlightRequest request)
        {
            lock (_lockObject)
            {
                var flight = _mapper.Map<Flight>(request);
                var validationResult = _validator.Validate(flight);

                if (!validationResult.IsValid) 
                {
                    return BadRequest();
                }

                var notSameFlight = _flightService.NotSameFlight(flight);
                if (!notSameFlight) 
                { 
                    return Conflict("Flight already exists.");
                }

                var result = _flightService.Create(flight);
                var response = _mapper.Map<FlightResponse>(flight);
                response.Id = result.Entity.Id;

                return Created("", response);
            }
        }

        [HttpDelete]
        [Route("flights/{id}")]
        public IActionResult DeleteFlight(int id)
        {
            lock (_lockObject)
            {
                var flight = _flightService.GetFullFlightById(id);
                if (flight == null)
                    return Ok(new { message = "Flight not found. Nothing to delete." });

                _flightService.Delete(flight);
                
               return Ok("Flight deleted successfully");
            }
        }
    }
}
