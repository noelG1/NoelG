using BankManagementSystemApi.Models;
using BankManagementSystemApi.Repository;
using BankManagementSystemIdentity.Models;
using BankManagementSystemMVC.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BankManagementSystemMVC.Controllers
{
    public class SharedController : Controller
    {
        private readonly AppDbContext context;
        private readonly UserManager<ApplicationUser> userManager;

        public SharedController(AppDbContext context,UserManager<ApplicationUser>userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }
        [HttpGet]
        public async Task<IActionResult> Profile(ApplicationUser user)
        {
            user = await userManager.FindByEmailAsync(User.Identity.Name);
            return View(user);
        }

        [HttpGet]
        public async Task<IActionResult> NewCustomers()
        {
            List<Customer> customers;
            customers = context.Customers.Where(x => x.registerDate.Date == DateTime.Today).ToList();
            return PartialView(customers);
        }
    }
}
