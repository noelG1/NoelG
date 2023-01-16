using BankManagementSystemIdentity.Models;
using BankManagementSystemMVC.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace BankManagementSystemMVC.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<ApplicationRole> roleManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        public EmployeesController(UserManager<ApplicationUser>userManager,SignInManager<ApplicationUser>signInManager,RoleManager<ApplicationRole>roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.signInManager = signInManager;
        }


        [HttpGet]
        public IActionResult Account()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Account(RegisterEmployeeViewModel register)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = register.email, firstName = register.firstName, lastName = register.lastName, city = register.city, gender = register.gender, Email = register.email, PhoneNumber = register.phoneNumber };
                var result = await userManager.CreateAsync(user, register.password);
                var role = new ApplicationRole { Name = register.role, roleDescription = "Role Description", creationTime = DateTime.Now };
                var resultRole = await roleManager.CreateAsync(role);
                if (result.Succeeded)
                {
                    await signInManager.SignInAsync(user, isPersistent: false);
                    await userManager.AddToRoleAsync(user, role.Name);
                    ViewBag.msg = "Registration was Successfull";
                }

                else
                {
                    ViewBag.msg = "Registration was Unsuccessfull";
                }


                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(register);
        }


       
    }
}
