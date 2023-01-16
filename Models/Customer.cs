using BankManagementSystemApi.Repository;
using System.ComponentModel.DataAnnotations;

namespace BankManagementSystemApi.Models
{
    public class Customer
    {
        
        public int id { get; set; }

        [Required]
        [Display(Name ="Picture")]
        public string picture { get; set; }

        [Display(Name = "Account Number")]
        public long accountNumber { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string firstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string lastName { get; set; }

        [Required]
        [Display(Name = "Email")]       
        public string email { get; set; }

        [Required]
        [Display(Name = "Phone Number")]
        public string phoneNumber { get; set; }

        [Required]
        [Display(Name = "Birth Date")]
        public DateTime birthDate { get; set; }

        [Required]
        [Display(Name = "City")]
        public string city { get; set; }

        [Required]
        [Display(Name = "Address")]
        public string address { get; set; }

        [Required]
        [Display(Name = "Account Type")]
        public AccountType accountType { get; set; }

        [Required]
        [Display(Name = "Balance")]
        public double balance { get; set; }

        [Display(Name = "Date Registered")]
        public DateTime registerDate { get; set; }
    }
        public enum AccountType 
        { 
            Saving,
            Checking
        }

}
