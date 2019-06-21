using System;
using System.Collections.Generic;
using System.Text;
using LunchTool.Service.DTO;

namespace LunchTool.Service.Interfaces
{
    public interface IOrderService
    {
        int MakeOrder(OrderDTO orderDTO);
        void AddOrderDish(OrderDishDTO orderDishDTO);
    }
}
