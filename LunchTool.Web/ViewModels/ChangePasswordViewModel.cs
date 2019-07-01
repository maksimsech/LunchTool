using System.ComponentModel.DataAnnotations;

namespace LunchTool.Web.ViewModels
{
    public class ChangePasswordViewModel
    {
        [Required]
        [MinLength(8)]
        [MaxLength(12)]
        public string OldPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [MinLength(8)]
        [MaxLength(12)]
        public string NewPassword { get; set; }

        [Required]
        [DataType((DataType.Password))]
        [Compare("NewPassword")]
        [MinLength(8)]
        [MaxLength(12)]
        public string ConfirmPassword { get; set; }
    }
}
