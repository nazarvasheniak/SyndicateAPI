﻿using System.Linq;

namespace SyndicateAPI.BusinessLogic.Interfaces
{
    public interface IBaseCrudService<T>
    {
        IQueryable<T> GetAll();
        T Get(long id);
        void Create(T item);
        void Update(T item);
        void Delete(T item);
        void Delete(long id);
    }
}
