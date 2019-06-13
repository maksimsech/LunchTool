using System;
using System.Collections.Generic;
using System.Text;

namespace LunchTool.Logic.Repository
{
    public interface IRepository<T> where T: class
    {
        IEnumerable<T> GetAll();
        T Get(int id);
        IEnumerable<T> Find(Func<T, bool> predicate);
        void Add(T item);
        void Update(T item);
        void Delete(int id);
    }
}
