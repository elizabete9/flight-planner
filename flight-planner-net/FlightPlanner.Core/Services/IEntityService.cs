﻿using FlightPlanner.Core.Models;

namespace FlightPlanner.Core.Services
{
    public interface IEntityService<T> where T : Entity
    {
        T? GetById(int id);

        ServiceResult Create(T entity);

        ServiceResult Delete(T entity);

        ServiceResult Update(T entity)  ;

        IEnumerable<T> List();
        
    }
}
