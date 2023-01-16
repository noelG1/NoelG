using BankManagementSystemApi.Models;
using System.ComponentModel.DataAnnotations;

namespace BankManagementSystemMVC.Models
{
    public class AddEntityViewModel
    {
        [Display(Name = "Picture")]
        public IFormFile? picture { get; set; }
        public string? pictureUrl { get; set; }

        [Display(Name = "First Name")]
        public string? firstName { get; set; }

        [Display(Name = "Last Name")]
        public string? lastName { get; set; }

        [Display(Name = "Email")]
        public string? email { get; set; }

        [Display(Name = "Phone Number")]
        public string? phoneNumber { get; set; }

        [Display(Name = "Address")]
        public string? address { get; set; }

        [Display(Name = "City")]
        public string? city { get; set; }

        [Display(Name = "Gender")]
        public string? gender { get; set; }

        [Display(Name = "Birth Date")]
        public DateTime birthDate { get; set; }

        [Display(Name = "Salary")]
        public double salary { get; set; }

        [Display(Name = "AccountType")]
        public AccountType accountType { get; set; }

        [Display(Name = "Balance")]
        public double balance { get; set; }

        [Display(Name = "Role")]
        public string role { get; set; }

        [Display(Name = "Password")]
        public string? password { get; set; }

        [Display(Name = "Confirm Password")]
        [Compare("password")]
        public string? confirmPassword { get; set; }
        public DateTime registerDate { get; set; }
    }
}
