using FlightPlanner.Core.Models;

namespace WebApplication1.Validations
{
    public class CarrierValidator : IValidator
    {
        public bool IsValid(Flight? flight)
        {
            return !string.IsNullOrEmpty(flight?.Carrier);
        }
    }
}
