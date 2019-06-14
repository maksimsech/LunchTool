using System;
using System.Collections.Generic;

namespace LunchTool.Logic.Repository.Interfaces
{
    public interface IRepository<T> where T: class
    {
        IEnumerable<T> GetAll();
        T Get(int id);
        IEnumerable<T> Find(Func<T, bool> predicate);
        void Add(T item);
        void Update(T item);
        void Delete(T item);
    }
}
