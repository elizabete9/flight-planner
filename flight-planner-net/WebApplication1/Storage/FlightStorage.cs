using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using WebApplication1.Database;
using WebApplication1.Models;

namespace WebApplication1.Storage
{
    public class FlightStorage(FlightPlannerDbContext context)
    {
        private readonly FlightPlannerDbContext _context = context;

        private static readonly object _lockObject = new object();

        public Flight GetFlightById(int id)
        {
            return _context.Flights
                .Include(flight => flight.From)
                .Include(flight => flight.To)
                .FirstOrDefault(flight => flight.Id == id);
        }


        public Flight AddFlight(AddFlightRequest flightRequest)
        {
            lock (_lockObject)
            {
                if (_context.Flights.Any(f =>
                f.DepartureTime.Trim().Equals(flightRequest.DepartureTime.Trim()) &&
                f.From.AirportCode.Trim().ToLower().Equals(flightRequest.From.AirportCode.Trim().ToLower()) &&
                f.To.AirportCode.Trim().ToLower().Equals(flightRequest.To.AirportCode.Trim().ToLower())))
                {
                    throw new InvalidOperationException("Flight already exists");
                }

                if (DateTime.TryParse(flightRequest.DepartureTime, out DateTime departure) &&
                DateTime.TryParse(flightRequest.ArrivalTime, out DateTime arrival))
                {
                    if (arrival <= departure)
                    {
                        throw new ArgumentException("Arrival time must be after departure time.");
                    }
                }
                else
                {
                    throw new ArgumentException("Invalid date format for DepartureTime or ArrivalTime.");
                }

                var flight = new Flight
                {
                    From = flightRequest.From,
                    To = flightRequest.To,
                    Carrier = flightRequest.Carrier,
                    DepartureTime = flightRequest.DepartureTime,
                    ArrivalTime = flightRequest.ArrivalTime
                };

                _context.Flights.Add(flight);
                _context.SaveChanges();

                return flight;
            }
        }
       
        public void ClearFlights()
        {
            _context.Flights.RemoveRange(_context.Flights);
            _context.Airports.RemoveRange(_context.Airports);
            _context.SaveChanges();
        }

        public PageResult<Flight> SearchFlights(SearchFlightsRequest searchRequest)
        {           
            var filteredFlights = _context.Flights
                .Where(flight =>
                flight.From.AirportCode.ToLower() == searchRequest.From.ToLower() &&
                flight.To.AirportCode.ToLower() == searchRequest.To.ToLower() &&
                flight.DepartureTime == searchRequest.DepartureDate)
                .ToList();

            return new PageResult<Flight>
            {
                Page = 0,
                TotalItems = filteredFlights.Count,
                Items = filteredFlights
            };
        }

        public bool DeleteFlight(int id)
        {
            lock (_lockObject)
            {
                var flight = _context.Flights.FirstOrDefault(f => f.Id == id);
                if (flight == null)
                    return false;

                _context.Remove(flight);
                _context.SaveChanges();

                return true;
            }
        }
    }
}
