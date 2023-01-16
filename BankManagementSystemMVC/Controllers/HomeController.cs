using BankManagementSystemIdentity.Models;
using BankManagementSystemMVC.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BankManagementSystemMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SignInManager<ApplicationUser> signInManager;

        public HomeController(ILogger<HomeController> logger,SignInManager<ApplicationUser>signInManager)
        {
            _logger = logger;
            this.signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult>Index(LoginViewModel login)
        {
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(login.email, login.password, false, false);
                if (result.Succeeded)
                {
                    if ( User.IsInRole("Customer Service Representative"))
                    {
                        return RedirectToAction("CustomerServiceRepresentativeHome", "CustomerServiceRepresentatives");
                    }
                    else if (User.IsInRole("Manager"))
                    {
                        return RedirectToAction("Index", "Managers");
                    }
                    else if ( User.IsInRole("Customer"))
                    {
                        return RedirectToAction("CustomerHome", "Customers");
                    }

                }

                else
                {
                    ViewBag.msg = "Invalid Login Attempt";
                }
            }
            return View(login);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}