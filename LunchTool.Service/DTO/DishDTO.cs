using System;
using System.Collections.Generic;
using System.Text;

namespace LunchTool.Service.DTO
{
    class DishDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Weight { get; set; }
        public decimal Price { get; set; }

        public int MenuId { get; set; }
    }
}
