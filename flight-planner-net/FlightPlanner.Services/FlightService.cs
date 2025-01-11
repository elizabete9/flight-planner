using FlightPlanner.Core.Models;
using FlightPlanner.Core.Services;
using FlightPlanner.Data;
using Microsoft.EntityFrameworkCore;

namespace FlightPlanner.Services
{
    public class FlightService : EntityService<Flight>, IFlightService
    {
        public FlightService(FlightPlannerDbContext context) : base (context)
        {  
        }

        public Flight? GetFullFlightById(int id)
        {
            return _context.Flights
                .Include(flight => flight.From)
                .Include(flight => flight.To)
                .SingleOrDefault(flight => flight.Id == id);
        }

        public bool NotSameFlight(Flight flight) 
        {
            if (_context.Flights.Any(f =>
                f.DepartureTime.Trim().Equals(flight.DepartureTime.Trim()) &&
                f.From.AirportCode.Trim().ToLower().Equals(flight.From.AirportCode.Trim().ToLower()) &&
                f.To.AirportCode.Trim().ToLower().Equals(flight.To.AirportCode.Trim().ToLower())))
            {
                return false;               
            }
            else
            {
                return true;
            }            
        }       
    }
}
