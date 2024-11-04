using WebApplication1.Models;

namespace WebApplication1.Storage
{
    public static class AirportService
    {
        private static List<Airport> _airports = new List<Airport>
        {
        new Airport { Country = "Latvia", City = "Riga", AirportCode = "RIX" },
        new Airport { Country = "Sweden", City = "Stockholm", AirportCode = "ARN" },
        new Airport { Country = "United Arab Emirates", City = "Dubai", AirportCode = "DXB" },
        new Airport { Country = "Russia", City = "Moscow", AirportCode = "DME" }
        };

        public static IEnumerable<Airport> SearchAirports(string search)
        {
            if (string.IsNullOrEmpty(search))
                return new List<Airport>();

            search = search.Trim().ToLower();

            return _airports.Where(airport =>
                airport.AirportCode.ToLower().Contains(search) ||
                airport.City.ToLower().Contains(search) ||
                airport.Country.ToLower().Contains(search)
            ).ToList();
        }
    }
}
