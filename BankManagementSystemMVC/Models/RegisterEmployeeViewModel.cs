using System.ComponentModel.DataAnnotations;

namespace BankManagementSystemMVC.Models
{
    public class RegisterEmployeeViewModel
    {
        [Required]
        public string firstName { get; set; }

        [Required]
        public string lastName { get; set; }


        [Required]
        public string city { get; set; }

        [Required]
        public string gender { get; set; }


      //  public double salary { get; set; }

        [Required]
        [StringLength(10, MinimumLength = 10)]
        public string phoneNumber { get; set; }

        [Required]
        [EmailAddress]
        public string email { get; set; }

        [Required]
        public string password { get; set; }

        [Required]
        public string confirmPassword { get; set; }

        [Required]
        public string role { get; set; }
    }
}
