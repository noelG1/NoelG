using BankManagementSystemApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BankManagementSystemApi.Repository
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext>options):base(options)
        {

        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Transaction>Transactions { get; set; }
        public DbSet<CustomerServiceRepresentative> CustomerServiceRepresentatives { get; set; }
    }
}
