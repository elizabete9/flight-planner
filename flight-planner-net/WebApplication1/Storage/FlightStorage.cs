using WebApplication1.Models;

namespace WebApplication1.Storage
{
    public static class FlightStorage
    {
        private static List<Flight> _flights = new List<Flight>();
        private static int _id = 0;
        private static readonly object _lockObject = new object();

        public static Flight GetFlightById(int id)
        {
            return _flights.FirstOrDefault(flight => flight.Id == id);
        }

        public static Flight AddFlight(AddFlightRequest flightRequest)
        {
            lock (_lockObject)
            {
                if (_flights.Any(f =>
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
                    Id = ++_id,
                    From = flightRequest.From,
                    To = flightRequest.To,
                    Carrier = flightRequest.Carrier,
                    DepartureTime = flightRequest.DepartureTime,
                    ArrivalTime = flightRequest.ArrivalTime
                };

                _flights.Add(flight);
                return flight;
            }
        }

        public static void ClearFlights()
        {
            _flights.Clear();
        }

        public static PageResult<Flight> SearchFlights(SearchFlightsRequest searchRequest)
        {
            DateTime.TryParse(searchRequest.DepartureDate, out var searchDate);

            var filteredFlights = _flights
                .Where(flight =>
                    flight.From.AirportCode.Equals(searchRequest.From, StringComparison.OrdinalIgnoreCase) &&
                    flight.To.AirportCode.Equals(searchRequest.To, StringComparison.OrdinalIgnoreCase) &&
                    DateTime.TryParse(flight.DepartureTime, out var departureDate) &&
                    departureDate.Date == searchDate.Date)
                .ToList();

            return new PageResult<Flight>
            {
                Page = 0,
                TotalItems = filteredFlights.Count,
                Items = filteredFlights
            };
        }

        public static bool DeleteFlight(int id)
        {
            lock (_lockObject)
            {
                var flight = _flights.FirstOrDefault(f => f.Id == id);
                if (flight == null)
                    return false;

                _flights.Remove(flight);
                return true;
            }
        }
    }
}
