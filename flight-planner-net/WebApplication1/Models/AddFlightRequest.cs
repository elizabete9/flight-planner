using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class AddFlightRequest
    {
        [Required]
        public Airport From { get; set; }

        [Required]
        public Airport To { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 1)]
        public string Carrier { get; set; }

        [Required]
        public string DepartureTime { get; set; }

        [Required]
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
