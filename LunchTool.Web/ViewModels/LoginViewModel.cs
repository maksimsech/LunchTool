using System.ComponentModel.DataAnnotations;

namespace LunchTool.Web.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [MinLength(8)]
        [MaxLength(12)]
        public string Password { get; set; }
    }
}
