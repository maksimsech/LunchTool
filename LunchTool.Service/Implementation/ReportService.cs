using AutoMapper;
using LunchTool.Logic.Entities;
using LunchTool.Logic.Repository.Implementation;
using LunchTool.Logic.Repository.Interfaces;
using LunchTool.Service.DTO;
using LunchTool.Service.Interfaces;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace LunchTool.Service.Implementation
{
    public class ReportService : IReportService
    {
        private IUnitOfWork db;
        private IMapper mapper;

        public ReportService(string connectionString)
        {
            db = new UnitOfWork(connectionString);
            mapper = new MapperConfiguration(cfg =>
            {
                
            }).CreateMapper();
        }

        public void Make(DateTime date)
        {
            var orders = db.Orders.Find(o => o.CreateDate.Date == date.Date);
            var orderDishes = db.OrderDishes.Find(od => orders.Any(o => o.Id == od.OrderId));
            decimal price = 0;
            foreach(var orderDish in orderDishes)
            {
                var dishPrice = db.Dishes.Find(d => d.Id == orderDish.DishId).Select(d => d.Price).FirstOrDefault();
                price += dishPrice * orderDish.Count;
            }
            //var userName = db.Users.Find(u => u.Id == )
        }
    }
}
