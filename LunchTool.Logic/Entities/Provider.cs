using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LunchTool.Logic.Entities
{
    public class Provider
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string Email { get; set; }
        public DateTime? StartWork { get; set; }
        public DateTime? EndWork { get; set; }
        [Required]
        public bool Active { get; set; }
        public string PhoneNumber { get; set; }

        public ICollection<Menu> Menus { get; set; }
    }
}
