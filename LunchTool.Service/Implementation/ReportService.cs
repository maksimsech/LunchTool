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
    public class ReportService 
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

        public IEnumerable<UserMonthReportDTO> UserMonthReport(DateTime date, int userId, int providerId)
        {
            IEnumerable<Order> orders;
            if (providerId == -1)
            {
                orders = db.Orders.Find(o => o.CreateDate.Month == date.Month && o.CreateDate.Year == date.Year && o.UserId == userId);
            }
            else
            {
                /* Find by provider */
                orders = db.Orders.Find(o => o.CreateDate.Month == date.Month && o.CreateDate.Year == date.Year && o.UserId == userId && GetOrderProviderId(o.Id) == providerId);
            }

            var daysInMonth = DateTime.DaysInMonth(date.Year, date.Month);

            var result = new List<UserMonthReportDTO>();

            for(int i = 1; i <= daysInMonth; i++)
            {
                var item = new UserMonthReportDTO { Day = i};

                var currentDateOrders = orders.Where(o => o.CreateDate.Day == i);

                var count = currentDateOrders.Count();

                decimal price = 0;
                foreach(var order in currentDateOrders)
                {
                    var orderDishes = db.OrderDishes.Find(od => od.OrderId == order.Id);
                    decimal orderPrice = 0;
                    foreach(var orderDish in orderDishes)
                    {
                        var dishPrice = db.Dishes.Find(d => d.Id == orderDish.DishId).Select(d => d.Price).SingleOrDefault();
                        dishPrice *= orderDish.Count;
                        orderPrice += dishPrice;
                    }
                    price += orderPrice;
                }
                item.OrderCount = count;
                item.Price = price;
                result.Add(item);
            }

            return result;
        }

        public void UserProvidersReport(int userId, DateTime fromDate, DateTime toDate)
        {
            var orders = db.Orders.Find(o => o.UserId == userId && o.CreateDate.Date >= fromDate.Date && o.CreateDate.Date <= toDate.Date);

            //Make more readable (provider id, OrderId)
            var providerOrders = new Dictionary<int, List<int>>();

            foreach (var order in orders)
            {
                var orderProviderId = GetOrderProviderId(order.Id);
                if (!providerOrders.Keys.Contains(orderProviderId))
                {
                    providerOrders.Add(orderProviderId, new List<int> { order.Id });
                }
                else
                {
                    providerOrders[orderProviderId].Add(order.Id);
                }
            }

            var result = new List<UserProvidersReportDTO>();

            foreach(var providerOrder in providerOrders)
            {
                var item = new UserProvidersReportDTO();
                //Get providerName
                var provider = db.Providers.Get(providerOrder.Key);
                item.ProviderName = provider.Name;

                decimal price = 0;
                foreach(var orderId in providerOrder.Value)
                {
                    var orderDishes = db.OrderDishes.Find(od => od.OrderId == orderId);
                    decimal orderPrice = 0;
                    foreach (var orderDish in orderDishes)
                    {
                        var dishPrice = db.Dishes.Find(d => d.Id == orderDish.DishId).Select(d => d.Price).SingleOrDefault();
                        dishPrice *= orderDish.Count;
                        orderPrice += dishPrice;
                    }
                    price += orderPrice;
                }

                item.OrderCount = providerOrder.Value.Count;
                item.Price = price;
            }
        }


        private int GetOrderProviderId(int orderId)
        {
            var dishId = db.OrderDishes.Find(od => od.OrderId == orderId).Select(od => od.DishId).FirstOrDefault();
            
            var menuId = db.Dishes.Find(d => dishId == d.Id).Select(d => d.MenuId).FirstOrDefault();

            var menu = db.Menus.Get(menuId);

            return menu.ProviderId;
        }

        private int GetProviderId(ProviderDTO providerDTO)  => db.Providers.Find(p => p.Name == providerDTO.Name && p.Email == providerDTO.Email).Select(p => p.Id).FirstOrDefault();      
    }
}
