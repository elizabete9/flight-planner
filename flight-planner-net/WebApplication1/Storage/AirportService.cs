using WebApplication1.Database;
using WebApplication1.Models;

namespace WebApplication1.Storage
{
    public class AirportService (FlightPlannerDbContext context)
    {
        private readonly FlightPlannerDbContext _context = context;


        public IEnumerable<Airport> SearchAirports(string search)
        {
            if (string.IsNullOrEmpty(search))
                return new List<Airport>();

            search = search.Trim().ToLower();

            return _context.Airports.Where(airport =>
                (!string.IsNullOrEmpty(airport.AirportCode) && airport.AirportCode.ToLower().Contains(search)) ||
                (!string.IsNullOrEmpty(airport.City) && airport.City.ToLower().Contains(search)) ||
                (!string.IsNullOrEmpty(airport.Country) && airport.Country.ToLower().Contains(search))
            ).ToList();
        }
    }
}
