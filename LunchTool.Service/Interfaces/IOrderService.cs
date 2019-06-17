using System;
using System.Collections.Generic;
using System.Text;
using LunchTool.Service.DTO;

namespace LunchTool.Service.Interfaces
{
    public interface IOrderService
    {
        void MakeOrder(OrderDTO orderDTO, OrderDishDTO orderDishDTO, int userId);
    }
}
