using System;
using System.Collections.Generic;
using System.Text;

namespace LunchTool.Logic.Enities
{
    public class MenuDish
    {
        public int Id { get; set; }

        public int MenuId { get; set; }
        public Menu Menu { get; set; }

        public int DishId { get; set; }
        public Dish Dish { get; set; }
    }
}
