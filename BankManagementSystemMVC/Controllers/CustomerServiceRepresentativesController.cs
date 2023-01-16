using BankManagementSystemApi.Models;
using BankManagementSystemApi.Services;
using BankManagementSystemIdentity.Models;
using BankManagementSystemMvc.Models;
using BankManagementSystemMVC.Models;
using FluentEmail.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Win32;
using Newtonsoft.Json;
using System.Net.Mail;

namespace BankManagementSystemMVC.Controllers
{
    
    public class CustomerServiceRepresentativesController : Controller
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<ApplicationRole> roleManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        public CustomerServiceRepresentativesController(IWebHostEnvironment webHostEnvironment, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager,SignInManager<ApplicationUser>signInManager)
        {
            this.webHostEnvironment = webHostEnvironment;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.signInManager = signInManager;
        }

        string baseUrl = "https://localhost:7277/";

        [Authorize(Roles ="Customer Service Representative")]
        public IActionResult CustomerServiceRepresentativeHome()
        {
            return View();
        }



        //Get All Customer Service Representatives
       [Authorize(Roles = "Manager")]
        public async Task<IActionResult> GetAllCustomerServiceRepresentatives()
        {
             List<CustomerServiceRepresentative> representatives = new List<CustomerServiceRepresentative>();
            CustomerServiceRepresentativeDoubleView rep = new CustomerServiceRepresentativeDoubleView();
            
              using (var client = new HttpClient())
              {
                  client.BaseAddress = new Uri(baseUrl);
                  client.DefaultRequestHeaders.Clear();
                  client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                  HttpResponseMessage response = await client.GetAsync("api/CustomerServiceRepresentatives");

                  if (response.IsSuccessStatusCode)
                  {
                      var result = response.Content.ReadAsStringAsync().Result;
                      representatives = JsonConvert.DeserializeObject<List<CustomerServiceRepresentative>>(result);
                      rep.representatives = representatives;
                  }
              }

            return View(rep);
                    
        }


        [HttpGet]
        [Authorize(Roles = "Manager")]
        public async Task<PartialViewResult> EditCustomerServiceRepresentative(int id)
        {
             CustomerServiceRepresentative representative = new CustomerServiceRepresentative();
             CustomerServiceRepresentativeDoubleView rep = new CustomerServiceRepresentativeDoubleView();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.GetAsync("api/CustomerServiceRepresentatives/"+id);

                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    representative = JsonConvert.DeserializeObject<CustomerServiceRepresentative>(result);
                    rep.representative = representative;
                   
                }
            }
            return PartialView(rep);


        }


        [HttpPost]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> EditCustomerServiceRepresentative(CustomerServiceRepresentativeDoubleView rep)
        {

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    if (rep.viewModel.picture != null)
                    {
                        string folder = "Images/CustomerServiceRepresentatives/";
                        string serverFolder = Path.Combine(webHostEnvironment.WebRootPath, folder);
                        if (!Directory.Exists(serverFolder))
                            Directory.CreateDirectory(serverFolder);
                        string virFilePath = folder + Guid.NewGuid().ToString() + rep.viewModel.picture.FileName;
                        string physicalFilePath = Path.Combine(webHostEnvironment.WebRootPath, virFilePath);
                        rep.viewModel.pictureUrl = virFilePath;


                        rep.viewModel.picture.CopyToAsync(new FileStream(physicalFilePath, FileMode.Create));

                    }

                    var editedRepresentative = new CustomerServiceRepresentative()
                    {
                        id= rep.representative.id,
                        picture = rep.viewModel.pictureUrl,
                        firstName = rep.representative.firstName,
                        lastName = rep.representative.lastName,
                        email = rep.representative.email,
                        phoneNumber = rep.representative.phoneNumber,
                        city=rep.representative.city,
                        address = rep.representative.firstName,
                        birthDate = rep.representative.birthDate,
                        salary = rep.representative.salary,
                        registerDate = DateTime.Now
                    };

                    HttpResponseMessage response =await client.PutAsJsonAsync(baseUrl + "api/CustomerServiceRepresentatives/" + editedRepresentative.id, editedRepresentative);


                    if (response.IsSuccessStatusCode)
                    {

                     ViewBag.msg = "Customer Service Representative Edited Successfully";
                        ModelState.Clear();
                    }

                    else
                    {
                        ViewBag.msg = "OOPS........Something Went Wrong, Please Try Again";
                        return PartialView(rep);
                    }
                }
            // }
            /* else
             {
                 var errors = ModelState.Select(x => x.Value.Errors).Where(y => y.Count > 0).ToList();
                 Console.WriteLine(errors);
             }*/
            return RedirectToAction("GetAllCustomerServiceRepresentatives", "CustomerServiceRepresentatives");
            
        }


        [HttpGet]
        public async Task<IActionResult> Profile(ApplicationUser user)
        {     
             user = await userManager.FindByEmailAsync(User.Identity.Name);
            return View(user);      
        }


        //Add A New Customer
        [HttpGet]
        [Authorize(Roles = "Manager")]
        public IActionResult AddCustomerServiceRepresentative()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> AddCustomerServiceRepresentative(AddCustomerServiceRepresentativeViewModel representative)
        {
            if (ModelState.IsValid)
            {
              
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    if (representative.picture != null)
                    {
                        string folder = "Images/CustomerServiceRepresentatives/";
                        string serverFolder = Path.Combine(webHostEnvironment.WebRootPath, folder);
                        if (!Directory.Exists(serverFolder))
                            Directory.CreateDirectory(serverFolder);
                        string virFilePath = folder + Guid.NewGuid().ToString() + representative.picture.FileName;
                        string physicalFilePath = Path.Combine(webHostEnvironment.WebRootPath, virFilePath);
                        representative.pictureUrl = virFilePath;


                        representative.picture.CopyToAsync(new FileStream(physicalFilePath, FileMode.Create));

                    }

                    var newRepresentative = new CustomerServiceRepresentative()
                    {
                        picture = representative.pictureUrl,
                        firstName = representative.firstName,
                        lastName = representative.lastName,
                        email = representative.email,
                        phoneNumber = representative.phoneNumber,
                        city = representative.city,
                        address = representative.address,
                        birthDate = representative.birthDate,
                        salary = representative.salary,
                        registerDate = DateTime.Now
                    };

                    HttpResponseMessage response = await client.PostAsJsonAsync(baseUrl + "api/CustomerServiceRepresentatives/", newRepresentative);

                    if (response.IsSuccessStatusCode)
                    {
                        var user = new ApplicationUser { UserName = representative.email, firstName = representative.firstName, lastName = representative.lastName, city = representative.city, gender = representative.gender, Email = representative.email, PhoneNumber = representative.phoneNumber };
                        var result = await userManager.CreateAsync(user, representative.password);
                        var role = new ApplicationRole { Name = "Customer Service Representative", roleDescription = "Role Description", creationTime = DateTime.Now };
                        var resultRole = await roleManager.CreateAsync(role);


                        if (result.Succeeded)
                        {
                          await userManager.AddToRoleAsync(user, role.Name);
                          ViewBag.msg = "Customer Service Representative Registered Successfully.";                          
                        }
                        else
                        {                         
                          ViewBag.msg = "Customer Service Representative Registered Successfully But Identity Was Not Set.";
                        } 

                      foreach(var error in result.Errors)
                      {
                       ModelState.AddModelError(string.Empty, error.Description);
                      }
                    }
                    else
                    {
                        ViewBag.msg = "OOPS........Something Went Wrong. Please Try Again!";
                        var console = await response.Content.ReadAsStringAsync();
                        Console.WriteLine("Errorrrrrrrrrrr" + console);
                    }
                     
                      
                }


               
            }
            
            return View(representative);
        
        }


        [HttpGet]
       [Authorize(Roles = "Manager")]
        public async Task<PartialViewResult> DeleteCustomerServiceRepresentative(int id)
        {
            CustomerServiceRepresentative representative = new CustomerServiceRepresentative();
            CustomerServiceRepresentativeDoubleView rep = new CustomerServiceRepresentativeDoubleView();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.GetAsync("api/CustomerServiceRepresentatives/" + id);

                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    representative = JsonConvert.DeserializeObject<CustomerServiceRepresentative>(result);
                    rep.representative = representative;

                }
            }
            return PartialView(rep);
        }


        [HttpPost]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> DeleteCustomerServiceRepresentative(CustomerServiceRepresentativeDoubleView rep)
        {

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.DeleteAsync("api/CustomerServiceRepresentatives/" + rep.representative.id);


                if (response.IsSuccessStatusCode)
                {

                    ViewBag.msg = "Customer Service Representative Deleted Successfully";
                    
                }

                else
                {
                    ViewBag.msg = "OOPS........Something Went Wrong, Please Try Again";
                }
            }

           return RedirectToAction("GetAllCustomerServiceRepresentatives");
        }


        [HttpGet]
        public PartialViewResult ResetPassword()
        {
            return PartialView();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(CustomerServiceRepresentativeDoubleView representative, string pass)
        {


            var user = userManager.FindByEmailAsync(User.Identity.Name);
            if (await userManager.CheckPasswordAsync(user.Result, pass))
            {
                await userManager.RemovePasswordAsync(user.Result);
                var changePassword = await userManager.AddPasswordAsync(user.Result, representative.viewModel.password);

                if (changePassword.Succeeded)
                {
                    return RedirectToAction("CustomerServiceRepresentativeHome", "CustomerServiceRepresentatives");
                }
                else
                {
                    foreach (var errors in changePassword.Errors)
                    {
                        ModelState.AddModelError(string.Empty, errors.Description);
                    }
                }
            }


            else
            {
                ViewBag.msg = "Incorrect Password";
            }


            return PartialView(representative);
        }

    }
}
