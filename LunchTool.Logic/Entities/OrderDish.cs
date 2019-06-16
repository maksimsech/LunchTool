using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace LunchTool.Logic.Entities
{
    public class OrderDish
    {
        public int Id { get; set; }
        [Required]
        public int Count { get; set; }

        public int OrderId { get; set; }
        public Order Order { get; set; }

        public int DishId { get; set; }
        public Dish Dish { get; set; }
    }
}
