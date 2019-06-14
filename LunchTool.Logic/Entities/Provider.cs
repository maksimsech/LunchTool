using System;
using System.Collections.Generic;

namespace LunchTool.Logic.Entities
{
    public class Provider
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public DateTime? StartWork { get; set; }
        public DateTime? EndWork { get; set; }
        public bool Active { get; set; }
        public string PhoneNumber { get; set; }

        public ICollection<Menu> Menus { get; set; }
    }
}
