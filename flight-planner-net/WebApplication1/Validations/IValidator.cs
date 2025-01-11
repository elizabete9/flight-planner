using FlightPlanner.Core.Models;

namespace WebApplication1.Validations
{
    public interface IValidator
    {
        bool IsValid(Flight? flight);
    }
}
