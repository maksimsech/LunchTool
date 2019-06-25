using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LunchTool.Logic.Entities
{
    public class Menu
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        public string Info { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public DateTime TimeLimit { get; set; }
        [Required]
        public bool IsActive { get; set; }

        public int ProviderId { get; set; }
        public Provider Provider { get; set; }

        public ICollection<Dish> Dishes { get; set; }
    }
}
