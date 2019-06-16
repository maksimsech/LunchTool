using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LunchTool.Logic.Entities
{
    public class Dish
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public double Weight { get; set; }
        [Required]
        public decimal Price { get; set; }

        public int MenuId { get; set; }
        public Menu Menu { get; set; }

        public virtual ICollection<OrderDish> OrderDishes { get; set; }

        public Dish()
        {
            OrderDishes = new List<OrderDish>();
        }
}
}
