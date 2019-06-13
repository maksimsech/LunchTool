using System;
using System.Collections.Generic;

namespace LunchTool.Logic.Enities
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime TimeLimit { get; set; }
        public DateTime Date { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public virtual ICollection<OrderDish> OrderDishes { get; set; }

        public Order()
        {
            OrderDishes = new List<OrderDish>();
        }
    }
}
