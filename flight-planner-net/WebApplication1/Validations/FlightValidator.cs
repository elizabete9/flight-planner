using FlightPlanner.Core.Models;
using FluentValidation;

namespace WebApplication1.Validations
{
    public class FlightValidator : AbstractValidator<Flight>
    {
        public FlightValidator()
        {
            RuleFor(flight => flight.ArrivalTime).NotEmpty();
            RuleFor(flight => flight.DepartureTime).NotEmpty();
            RuleFor(flight => flight.DepartureTime).Must((flight, departureTime) => 
                DateTime.Parse(departureTime) < DateTime.Parse(flight.ArrivalTime))
                .When(flight => !string.IsNullOrEmpty(flight.ArrivalTime) && !string.IsNullOrEmpty(flight.DepartureTime));
            RuleFor(flight => flight.Carrier).NotEmpty();
            RuleFor(flight => flight.From).NotNull();
            RuleFor(flight => flight.To).NotNull();
            RuleFor(flight => flight.From.AirportCode).NotEmpty();
            RuleFor(flight => flight.To.AirportCode).NotEmpty();
            RuleFor(flight => flight.From.AirportCode)
                 .Must((flight, fromCode) =>
                    !string.Equals(fromCode, flight.To.AirportCode, StringComparison.OrdinalIgnoreCase));
            RuleFor(flight => flight.From.Country)
                .Must((flight, fromCountry) =>
                    !string.Equals(fromCountry, flight.To.Country, StringComparison.OrdinalIgnoreCase));
            RuleFor(flight => flight.From.City)
                .Must((flight, fromCity) =>
                    !string.Equals(fromCity, flight.To.City, StringComparison.OrdinalIgnoreCase));
                
        }
    }
}
