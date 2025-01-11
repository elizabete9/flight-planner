using FlightPlanner.Core.Models;

namespace FlightPlanner.Core.Services
{
    public interface IDbService
    {
        T? GetById<T>(int id) where T : Entity;

        ServiceResult Create<T>(T entity) where T : Entity;

        ServiceResult Delete<T>(T entity) where T : Entity;

        ServiceResult Update<T>(T entity) where T : Entity;

        IEnumerable<T> List<T>() where T : Entity;
    }
}
