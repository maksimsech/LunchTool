using System;
using System.Collections.Generic;
using System.Text;

namespace LunchTool.Service.DTO
{
    public class ProviderDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public DateTime? StartWork { get; set; }
        public DateTime? EndWork { get; set; }
        public bool Active { get; set; }
        public string PhoneNumber { get; set; }
    }
}
