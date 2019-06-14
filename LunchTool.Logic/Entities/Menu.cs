using System;
using System.Collections.Generic;

namespace LunchTool.Logic.Entities
{
    public class Menu
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Info { get; set; }
        public DateTime Date { get; set; }
        public DateTime TimeLimit { get; set; }

        public int ProviderId { get; set; }
        public Provider Provider { get; set; }

        public ICollection<Dish> Dishes { get; set; }
    }
}
