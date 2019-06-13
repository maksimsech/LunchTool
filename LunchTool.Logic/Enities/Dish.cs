using System.Collections.Generic;

namespace LunchTool.Logic.Enities
{
    public class Dish
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Weight { get; set; }
        public decimal Price { get; set; }

        public virtual ICollection<OrderDish> OrderDishes { get; set; }
        public virtual ICollection<MenuDish> MenuDishes { get; set; }

        public Dish()
        {
            OrderDishes = new List<OrderDish>();
            MenuDishes = new List<MenuDish>();
        }
}
}
