using System.ComponentModel.DataAnnotations;

namespace LunchTool.Web.ViewModels
{
    public class UserViewModel
    {
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string Patronymic { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsAdmin { get; set; }
        public string PhoneNumber { get; set; }
    }
}
