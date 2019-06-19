using System;
using System.ComponentModel.DataAnnotations;

namespace LunchTool.Web.ViewModels
{
    public class ProviderViewModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string Email { get; set; }
        public DateTime? StartWork { get; set; }
        public DateTime? EndWork { get; set; }
        public bool Active { get; set; }
        public string PhoneNumber { get; set; }
    }
}
