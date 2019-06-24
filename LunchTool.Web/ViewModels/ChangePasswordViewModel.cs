using System.ComponentModel.DataAnnotations;

namespace LunchTool.Web.ViewModels
{
    public class ChangePasswordViewModel
    {
        [Required]
        public string OldPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required]
        [DataType((DataType.Password))]
        [Compare("NewPassword")]
        public string ConfirmPassword { get; set; }
    }
}
