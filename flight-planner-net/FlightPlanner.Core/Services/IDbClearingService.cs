using FlightPlanner.Core.Models;

namespace FlightPlanner.Core.Services
{
    public interface IDbClearingService : IDbService
    {
        ServiceResult Clear<T>() where T : Entity;
    }
}
