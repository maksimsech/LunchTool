using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using LunchTool.Logic.Entities;
using LunchTool.Logic.Context;
using LunchTool.Logic.Repository.Interfaces;

namespace LunchTool.Logic.Repository.Implementation
{
    public class OrderRepository: IRepository<Order>
    {
        private DataContext dataContext;

        public OrderRepository(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public IEnumerable<Order> GetAll() => dataContext.Orders;

        public Order Get(int id) => dataContext.Orders.Find(id);

        public void Add(Order item) => dataContext.Orders.Add(item);

        public void Update(Order item) => dataContext.Entry(item).State = EntityState.Modified;

        public IEnumerable<Order> Find(Func<Order, bool> predicate) => dataContext.Orders.Where(predicate).ToList();

        public void Delete(Order item) => dataContext.Orders.Remove(item);
    }
}
