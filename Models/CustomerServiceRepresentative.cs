using System.ComponentModel.DataAnnotations;

namespace BankManagementSystemApi.Models
{
    public class CustomerServiceRepresentative
    {
        public int id { get; set; }

        [Display(Name = "Picture")]
        public string  picture{ get; set; }

        [Display(Name = "First Name")]
        public string firstName { get; set; }

        [Display(Name = "Last Name")]
        public string lastName { get; set; }

        [Display(Name = "Email")]
        public string email { get; set; }

        [Display(Name = "Phone Number")]
        public string phoneNumber { get; set; }

        [Display(Name = "Birth Date")]
        public DateTime birthDate { get; set; }

        [Display(Name = "City")]
        public string city { get; set; }

        [Display(Name = "Address")]
        public string address { get; set; }

        [Display(Name = "Salary")]
        public double salary { get; set; }
        public DateTime registerDate { get; set; }
    }
}
