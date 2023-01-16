using Microsoft.AspNetCore.Identity;

namespace BankManagementSystemIdentity.Models
{
    public class ApplicationRole:IdentityRole
    {
       public ApplicationRole()
        {

        }
        public ApplicationRole(string roleName) : base(roleName)
        {

        }
        public ApplicationRole(string roleName, string roleDescription, DateTime creationTime) : base(roleName)
        {
            this.roleDescription = roleDescription;
            this.creationTime = creationTime;
        }

        public string roleDescription { get; set; }
        public DateTime creationTime { get; set; }
    }
}
