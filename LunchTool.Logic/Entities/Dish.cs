using System.Collections.Generic;

namespace LunchTool.Logic.Entities
{
    public class Dish
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Weight { get; set; }
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
