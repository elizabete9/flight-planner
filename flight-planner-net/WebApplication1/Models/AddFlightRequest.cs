using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class AddFlightRequest
    {
        public Airport From { get; set; }

        public Airport To { get; set; }

        [StringLength(100, MinimumLength = 1)]
        public string Carrier { get; set; }

        public string DepartureTime { get; set; }

        public string ArrivalTime { get; set; }
       
        public bool IsSameAirport()
        {
            return string.Equals(From.Country, To.Country, StringComparison.OrdinalIgnoreCase) &&
                   string.Equals(From.City, To.City, StringComparison.OrdinalIgnoreCase) &&
                   string.Equals(From.AirportCode.Trim(), To.AirportCode.Trim(), StringComparison.OrdinalIgnoreCase);
        }

        public bool HasValidDates()
        {
            DateTime.TryParse(DepartureTime, out DateTime departure);
            DateTime.TryParse(ArrivalTime, out DateTime arrival);
            return arrival > departure;
        }
    }
}
