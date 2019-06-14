using System;
using System.Collections.Generic;
using System.Text;
using LunchTool.Logic.Entities;

namespace LunchTool.Logic.Repository.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Dish> Dishes { get; }
        IRepository<Menu> Menus { get; }
        IRepository<Order> Orders { get; }
        IRepository<Provider> Providers { get; }
        IRepository<User> Users { get; }

        void Save();
    }
}
