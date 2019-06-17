using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LunchTool.Service.DTO
{
    public class ProviderDTO
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
    }
}
