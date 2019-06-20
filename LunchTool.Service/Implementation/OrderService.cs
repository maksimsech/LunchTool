using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using LunchTool.Service.DTO;
using LunchTool.Service.Interfaces;
using LunchTool.Logic.Entities;
using LunchTool.Logic.Repository.Interfaces;
using LunchTool.Logic.Repository.Implementation;
using LunchTool.Service.Infrastracture;
using AutoMapper;

namespace LunchTool.Service.Implementation
{
    public class OrderService : IOrderService
    {
        private IUnitOfWork db;
        private IMapper mapper;

        public OrderService(string connectionString)
        {
            db = new UnitOfWork(connectionString);
            mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<OrderDTO, Order>();
                cfg.CreateMap<OrderDishDTO, Order>();
            }).CreateMapper();
        }

        public void MakeOrder(OrderDTO orderDTO, OrderDishDTO orderDishDTO, int userId)
        {
            orderDTO.UserId = userId;
            var order = mapper.Map<OrderDTO, Order>(orderDTO);
            db.Orders.Add(order);
            var orderDish = mapper.Map<OrderDishDTO, OrderDish>(orderDishDTO);
            db.OrderDishes.Add(orderDish);
            db.Save();
        }
    }
}
