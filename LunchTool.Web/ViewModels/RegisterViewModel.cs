using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace LunchTool.Web.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public string Patronymic { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType((DataType.Password))]
        public string Password { get; set; }

        [Required]
        [DataType((DataType.Password))]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
 
        public bool IsAdmin { get; set; }

        public string PhoneNumber { get; set; }
    }
}
