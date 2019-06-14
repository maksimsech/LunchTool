using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using LunchTool.Logic.Entities;
using LunchTool.Logic.Context;
using LunchTool.Logic.Repository.Interfaces;

namespace LunchTool.Logic.Repository.Implementation
{
    public class UserRepository : IRepository<User> 
    {
        private DataContext dataContext;

        public UserRepository(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public IEnumerable<User> GetAll() => dataContext.Users;

        public User Get(int id) => dataContext.Users.Find(id);

        public void Add(User item) => dataContext.Users.Add(item);

        public void Update(User item) => dataContext.Entry(item).State = EntityState.Modified;

        public IEnumerable<User> Find(Func<User, bool> predicate) => dataContext.Users.Where(predicate).ToList();

        public void Delete(int id)
        {
            var user = dataContext.Users.Find(id);
            if (user != null)
                dataContext.Users.Remove(user);
        }

    }
}
