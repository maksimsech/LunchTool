using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LunchTool.Web.ViewModels
{
    public class CopyDishViewModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public double Weight { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public bool IsSelected { get; set; }
    }
}
