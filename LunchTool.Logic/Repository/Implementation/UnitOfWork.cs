using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using LunchTool.Logic.Repository.Interfaces;
using LunchTool.Logic.Context;
using LunchTool.Logic.Entities;

namespace LunchTool.Logic.Repository.Implementation
{
    public class UnitOfWork : IUnitOfWork
    {
        private DataContext dataContext;
        private DishRepository dishRepository;
        private MenuRepository menuRepository;
        private OrderRepository orderRepository;
        private ProviderRepository providerRepository;
        private UserRepository userRepository;

        public UnitOfWork(string connectionString)
        {
            var dbOptions = new DbContextOptionsBuilder<DataContext>();
            dbOptions.UseSqlServer(connectionString);
            dataContext = new DataContext(dbOptions.Options);
        }

        public IRepository<Dish> Dishes { get {
                if (dishRepository == null)
                    dishRepository = new DishRepository(dataContext);
                return dishRepository;
            } }

        public IRepository<Menu> Menus { get {
                if (menuRepository == null)
                    menuRepository = new MenuRepository(dataContext);
                return menuRepository;
            } }

        public IRepository<Order> Orders { get {
                if (orderRepository == null)
                    orderRepository = new OrderRepository(dataContext);
                return orderRepository;
            } }

        public IRepository<Provider> Providers { get {
                if (providerRepository == null)
                    providerRepository = new ProviderRepository(dataContext);
                return providerRepository;
            } }

        public IRepository<User> Users { get {
                if (userRepository == null)
                    userRepository = new UserRepository(dataContext);
                return userRepository;
            } }

        public void Save() => dataContext.SaveChanges();

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    dataContext.Dispose();
                }
                disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
