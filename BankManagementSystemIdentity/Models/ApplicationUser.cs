using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace BankManagementSystemIdentity.Models
{
    public class ApplicationUser:IdentityUser
    {
        public ApplicationUser()
        {

        }
     
        [Display(Name = "First Name")]
        
        public string firstName { get; set; }

   
        [Display(Name = "Last Name")]
      
        public string lastName { get; set; }


        [Display(Name = "City")]
        public string city { get; set; }

        [Display(Name = "Gender")]
        public string gender { get; set; }

        [Display(Name = "Salary")]
        public double salary { get; set; }

      
    }
}
