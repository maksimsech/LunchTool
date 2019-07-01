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
using LunchTool.Service.Infrastracture;

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
                orders = db.Orders.Find(o => o.CreateDate.Month == date.Month && 
                                        o.CreateDate.Year == date.Year && 
                                        o.UserId == userId && 
                                        GetOrderProviderId(o.Id) == providerId);
            }

            var daysInMonth = DateTime.DaysInMonth(date.Year, date.Month);

            var result = new List<UserMonthReportDTO>();

            for(int i = 1; i <= daysInMonth; i++)
            {
                var item = new UserMonthReportDTO { Day = i};

                var currentDateOrdersId = orders.Where(o => o.CreateDate.Day == i).Select(o => o.Id);

                var count = currentDateOrdersId.Count();

                decimal price = PriceOfOrders(currentDateOrdersId);
                item.OrderCount = count;
                item.Price = price;
                result.Add(item);
            }

            return result;
        }

        public IEnumerable<UserProvidersReportDTO> UserProvidersReport(int userId, DateTime fromDate, DateTime toDate)
        {
            var ordersId = db.Orders.Find(o => o.UserId == userId && o.CreateDate.Date >= fromDate.Date && o.CreateDate.Date <= toDate.Date).Select(o => o.Id);

            var user = db.Users.Get(userId);

            var userName = $"{user.LastName} {user.FirstName} {user.Patronymic?? ""}";

            //Make more readable (provider id, OrderId)
            var providerOrders = new Dictionary<int, List<int>>();

            foreach (var orderId in ordersId)
            {
                var orderProviderId = GetOrderProviderId(orderId);
                if (!providerOrders.Keys.Contains(orderProviderId))
                {
                    providerOrders.Add(orderProviderId, new List<int> { orderId });
                }
                else
                {
                    providerOrders[orderProviderId].Add(orderId);
                }
            }

            var result = new List<UserProvidersReportDTO>();

            foreach(var providerOrder in providerOrders)
            {
                var item = new UserProvidersReportDTO();
                //Get providerName
                var provider = db.Providers.Get(providerOrder.Key);
                item.ProviderName = provider.Name;

                decimal price = PriceOfOrders(providerOrder.Value);

                item.OrderCount = providerOrder.Value.Count;
                item.Price = price;
                item.UserName = userName;
                result.Add(item);
            }

            return result;
        }

        public IEnumerable<AllUsersReportDTO> AllUsersReport(int providerId, DateTime fromDate, DateTime toDate)
        {
            IEnumerable<Order> orders;
            if (providerId == -1)
            {
                orders = db.Orders.Find(o => o.CreateDate.Date >= fromDate && o.CreateDate.Date <= toDate);
            }
            else
            {
                orders = db.Orders.Find(o => o.CreateDate.Date >= fromDate && o.CreateDate.Date <= toDate && GetOrderProviderId(o.Id) == providerId);
            }
            var userOrders = new Dictionary<int, List<int>>();
            foreach(var order in orders)
            {
                var userId = order.UserId;
                if (!userOrders.Keys.Contains(userId))
                {
                    userOrders.Add(userId, new List<int> { order.Id });
                }
                else
                {
                    userOrders[userId].Add(order.Id);
                }
            }

            var result = new List<AllUsersReportDTO>();

            foreach(var userOrder in userOrders)
            {
                var item = new AllUsersReportDTO();

                var user = db.Users.Get(userOrder.Key);
                var userName = $"{user.LastName} {user.FirstName} {user.Patronymic?? ""}";
                item.UserName = userName;

                decimal price = PriceOfOrders(userOrder.Value);
                var count = userOrder.Value.Count;
                item.Price = price;
                item.OrderCount = count;
                result.Add(item);
            }

            return result;
        }

        public IEnumerable<UserPageReportDTO> GetUserOrders(int userId, DateTime fromDate, DateTime toDate)
        {
            var orders = db.Orders.Find(o => o.UserId == userId && o.CreateDate.Date >= fromDate.Date && o.CreateDate.Date <= toDate.Date);

            var result = new List<UserPageReportDTO>();
            foreach(var order in orders)
            {
                var item = new UserPageReportDTO();
                item.OrderDate = order.CreateDate;
                var providerId = GetOrderProviderId(order.Id);
                var provider = db.Providers.Get(providerId);
                item.ProviderName = provider.Name;

                var dishNamesSB = new StringBuilder();
                decimal price = 0;
                var orderDishes = order.OrderDishes;
                foreach(var orderDish in orderDishes)
                {
                    var dish = orderDish.Dish;
                    var dishPrice = dish.Price;
                    var dishName = dish.Name;
                    dishNamesSB.Append(dishName + (orderDish.Count > 1? "(" + orderDish.Count.ToString() + ")" : "") + ", ");
                    price += dishPrice * orderDish.Count;
                }
                var dishNames = dishNamesSB.ToString();
                dishNames = dishNames.Substring(0, dishNames.Length - 2);
                dishNames += '.';
                item.Price = price;
                item.Dishes = dishNames;
                result.Add(item);
            }

            result.Sort((i, j) => DateTime.Compare(i.OrderDate, j.OrderDate));
            return result;
        }

        private decimal PriceOfOrders(IEnumerable<int> ordersId)
        {
            decimal price = 0;
            foreach(var orderId in ordersId)
            {
                var orderDishes = db.OrderDishes.Find(od => od.OrderId == orderId);
                decimal orderPrice = 0;
                foreach(var orderDish in orderDishes)
                {
                    decimal? dishPrice = db.Dishes.Find(d => d.Id == orderDish.DishId).Select(d => d.Price).SingleOrDefault();
                    if (!dishPrice.HasValue)
                    {
                        throw new ValidationException("dish", "Нет заказанного блюда");
                    }
                    orderPrice += dishPrice.Value * orderDish.Count;
                }
                price += orderPrice;
            }
            return price;
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
