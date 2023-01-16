using BankManagementSystemIdentity.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BankManagementSystemIdentity.Repository
{
    public class AppDbContext:IdentityDbContext<ApplicationUser,ApplicationRole,string>
    {
        public AppDbContext(DbContextOptions<AppDbContext>options):base(options)
        {

        }
        
    }
}
