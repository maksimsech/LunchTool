using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LunchTool.Web.ViewModels
{
    public class DishViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [DisplayFormat(DataFormatString = "{0:n0}")]
        public double Weight { get; set; }
        [DisplayFormat(DataFormatString = "{0:n0}")]
        public decimal Price { get; set; }

        public int MenuId { get; set; }
    }
}
