using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using LunchTool.Logic.Entities;
using LunchTool.Logic.Context;
using LunchTool.Logic.Repository.Interfaces;

namespace LunchTool.Logic.Repository.Implementation
{
    class GenericRepository<T> : IRepository<T> where T : class
    {
        DataContext dataContext;
        DbSet<T> dbSet;

        public GenericRepository(DataContext dataContext)
        {
            this.dataContext = dataContext;
            dbSet = dataContext.Set<T>();
        }

        public IEnumerable<T> GetAll() => dbSet.ToList();

        public T Get(int id) => dbSet.Find(id);

        public IEnumerable<T> Find(Func<T, bool> predicate) => dbSet.Where(predicate).ToList();

        public void Add(T item) => dbSet.Add(item);

        public void Update(T item) => dataContext.Entry(item).State = EntityState.Modified;

        public void Delete(T item) => dbSet.Remove(item);
    }
}
