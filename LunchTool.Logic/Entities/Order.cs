using System;
using System.Collections.Generic;

namespace LunchTool.Logic.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime CreateDate { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public virtual ICollection<OrderDish> OrderDishes { get; set; }

        public Order()
        {
            OrderDishes = new List<OrderDish>();
        }
    }
}
