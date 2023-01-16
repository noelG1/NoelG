using System.ComponentModel.DataAnnotations;

namespace BankManagementSystemMVC.Models
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name ="Email")]
        public string email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name ="Password")]
        public string password { get; set; }
    }
}
