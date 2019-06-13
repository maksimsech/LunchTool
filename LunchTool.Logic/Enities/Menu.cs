using System;
using System.Collections.Generic;

namespace LunchTool.Logic.Enities
{
    public class Menu
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Info { get; set; }
        public DateTime Date { get; set; }

        public int ProviderId { get; set; }
        public Provider Provider { get; set; }

        public virtual ICollection<MenuDish> MenuDishes { get; set; }

        public Menu()
        {
            MenuDishes = new List<MenuDish>();
        }
    }
}
