using BankManagementSystemApi.Models;
using BankManagementSystemApi.Services;
using BankManagementSystemIdentity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using BankManagementSystemMVC.Models;
using BankManagementSystemMvc.Models;
using FluentEmail.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Data.SqlClient;

namespace BankManagementSystemMVC.Controllers
{
    public class CustomersController : Controller
    {
        public CustomersController(IWebHostEnvironment webHostEnvironment,UserManager<ApplicationUser>userManager,RoleManager<ApplicationRole>roleManager,SignInManager<ApplicationUser>signInManager)
        {
            this.webHostEnvironment = webHostEnvironment;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.signInManager = signInManager;
        }
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<ApplicationRole> roleManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        string baseUrl = "https://localhost:7277/";


        [Authorize(Roles = "Customer")]
        public IActionResult CustomerHome()
        {
            return View();
        }


        //Get All Customers
        [Authorize(Roles = "Manager,Customer Service Representative")]
        public async Task<IActionResult> GetAllCustomers()
        {
            List<Customer> customers = new List<Customer>();
            CustomerDoubleView cust = new CustomerDoubleView();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.GetAsync("api/customers");

                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    customers = JsonConvert.DeserializeObject<List<Customer>>(result);
                    cust.customers = customers;
                }
            }
            return View(cust);
        }

        //Get A Single Customer Using Id
        [HttpGet]
        public async Task<IActionResult> GetCustomerById(int id)
        {
            Customer customer = new Customer();
            CustomerDoubleView cust = new CustomerDoubleView();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.GetAsync("api/customers/" +id);

                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    customer = JsonConvert.DeserializeObject<Customer>(result);
                    cust.customer = customer;
                }
            }
            return View(cust);
        }

        //Get A Single Customer Using Account Number
        [HttpGet]
        public async Task<IActionResult> GetCustomerByAccountNumber(long accountNumber)
        {
          //  Transaction transaction= new Transaction();
          //  accountNumber = transaction.receiver;
            Customer customer = new Customer();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.GetAsync("api/customers/GetSingleByAccountNumber/" + accountNumber);

                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    customer = JsonConvert.DeserializeObject<Customer>(result);
                }
            }
            return PartialView(customer);
        }

        //Add A New Customer
        [HttpGet]
        [Authorize(Roles = "Manager,Customer Service Representative")]
        public IActionResult AddCustomer()
        {
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "Manager,Customer Service Representative")]
        public async Task<IActionResult> AddCustomer(AddCustomerViewModel customer,[FromServices]IFluentEmail email)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));


                if (customer.picture != null)
                {
                    string folder = "Images/Customers/";
                    string serverFolder = Path.Combine(webHostEnvironment.WebRootPath, folder);

                    if (!Directory.Exists(serverFolder))
                        Directory.CreateDirectory(serverFolder);


                    string virFilePath = folder + Guid.NewGuid().ToString() + customer.picture.FileName;
                    string physicalFilePath = Path.Combine(webHostEnvironment.WebRootPath, virFilePath);
                    customer.pictureUrl = virFilePath;


                    customer.picture.CopyToAsync(new FileStream(physicalFilePath, FileMode.Create));

                }

                var newCustomer = new Customer()
                {
                    picture = customer.pictureUrl,
                    firstName = customer.firstName,
                    lastName = customer.lastName,
                    email = customer.email,
                    phoneNumber = customer.phoneNumber,
                    city= customer.city,
                    address = customer.address,
                    birthDate = customer.birthDate,
                    accountType = customer.accountType,
                    balance = customer.balance,
                    registerDate = DateTime.Now
                };

                HttpResponseMessage response = await client.PostAsJsonAsync(baseUrl + "api/customers/", newCustomer);

                if (response.IsSuccessStatusCode)
                {
                    var user = new ApplicationUser { UserName = customer.email, firstName = customer.firstName, lastName = customer.lastName, city = customer.city, gender = customer.gender, Email = customer.email, PhoneNumber = customer.phoneNumber };
                    var result = await userManager.CreateAsync(user, customer.password);
                    var role = new ApplicationRole { Name = "Customer", roleDescription = "Bank Customer", creationTime = DateTime.Now };
                    var resultRole = await roleManager.CreateAsync(role);



                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, role.Name);
                        ViewBag.msg = "Customer Registered Successfully.";
                    }
                    else
                    {
                        ViewBag.msg = "Customer Service Representative Registered Successfully But Identity Was Not Set.";
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }


                    /*   var sendEmail=await email
                            .To("noel.girma@gmail.com")
                            .Subject("Password")
                            .Body("pass")
                            .SendAsync();
                        if (!sendEmail.Successful)
                        {
                            Console.WriteLine("Not successful");
                        }*/
                   
                }
                else
                {
                    ViewBag.msg = "OOPS........Something Went Wrong. Please Try Again";
                    var console = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("Errorrrrrrrrrrr" + console);
                }
                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    var body = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("blahblahblah ERROR" + body);
                }

            }

            return View(customer);
        }


        //Get And Edit Customer
        [HttpGet]
        [Authorize(Roles = "Manager,Customer Service Representative")]
        public async Task<PartialViewResult> EditCustomer(int id)
        {
            Customer customer = new Customer();
            CustomerDoubleView cust = new CustomerDoubleView();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync("api/customers/GetSingleById/" + id);
                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    customer = JsonConvert.DeserializeObject<Customer>(result);
                    cust.customer = customer;
                }
            }
            return PartialView(cust);
        }


        [HttpPost]
        [Authorize(Roles = "Manager,Customer Service Representative")]
        public async Task<IActionResult> EditCustomer(CustomerDoubleView cust)
        {
  
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                if (cust.viewModel.picture != null)
                {
                    string folder = "Images/Customers/";
                    string serverFolder = Path.Combine(webHostEnvironment.WebRootPath, folder);
                    if (!Directory.Exists(serverFolder))
                        Directory.CreateDirectory(serverFolder);
                    string virFilePath = folder + Guid.NewGuid().ToString() + cust.viewModel.picture.FileName;
                    string physicalFilePath = Path.Combine(webHostEnvironment.WebRootPath, virFilePath);
                    cust.viewModel.pictureUrl = virFilePath;


                    cust.viewModel.picture.CopyToAsync(new FileStream(physicalFilePath, FileMode.Create));

                }

                var editedCustomer = new Customer()
                {
                    id = cust.customer.id,
                    picture = cust.viewModel.pictureUrl,
                    firstName = cust.customer.firstName,
                    lastName = cust.customer.lastName,
                    email = cust.customer.email,
                    phoneNumber = cust.customer.phoneNumber,
                    city = cust.customer.city,
                    address = cust.customer.address,
                    birthDate = cust.customer.birthDate,
                    accountType=cust.customer.accountType,
                    registerDate = DateTime.Now
                };

                HttpResponseMessage response = await client.PutAsJsonAsync(baseUrl + "api/Customers/" + editedCustomer.id, editedCustomer);


                if (response.IsSuccessStatusCode)
                {                
                    ViewBag.msg = "Customer Edited Successfully";
                    return RedirectToAction("GetAllCustomers");
                }

                else
                {
                    ViewBag.msg = "OOPS........Something Went Wrong, Please Try Again";
                }
            }

            return View(cust);
        }



        [HttpGet]
        [Authorize(Roles = "Manager,Customer Service Representative")]
        public async Task<PartialViewResult> DeleteCustomer(int id)
        {
            Customer customer = new Customer();
            CustomerDoubleView cust = new CustomerDoubleView();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.GetAsync("api/Customers/GetSingleById/" + id);

                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    customer = JsonConvert.DeserializeObject<Customer>(result);
                    cust.customer = customer;

                }
            }
            return PartialView(cust);
        }


        [HttpPost]
        [Authorize(Roles = "Manager,Customer Service Representative")]
        public async Task<IActionResult> DeleteCustomer(CustomerDoubleView cust)
        {

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.DeleteAsync("api/Customers/" + cust.customer.id);


                if (response.IsSuccessStatusCode)
                {

                    ViewBag.msg = "Customer Deleted Successfully";
                    ModelState.Clear();
                }

                else
                {
                    ViewBag.msg = "OOPS........Something Went Wrong, Please Try Again";
                }
            }

            return RedirectToAction("GetAllCustomers");
        }


        public async Task<PartialViewResult> Profile(ApplicationUser user)
        {
            user = await userManager.FindByEmailAsync(User.Identity.Name);
            return PartialView(user);
        }

        
        public PartialViewResult ResetPassword()
        {
            return PartialView();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(CustomerDoubleView customer,string pass)
        {

            
            var user = userManager.FindByEmailAsync(User.Identity.Name);
            if (await userManager.CheckPasswordAsync(user.Result,pass))
            {
              await userManager.RemovePasswordAsync(user.Result);
              var changePassword=await userManager.AddPasswordAsync(user.Result, customer.viewModel.password);

                if (changePassword.Succeeded)
                {
                    return RedirectToAction("CustomerHome","Customers");
                }
                else
                {
                    foreach(var errors in changePassword.Errors)
                    {
                        ModelState.AddModelError(string.Empty, errors.Description);
                    }
                }
            }
            

            else
              {
                ViewBag.msg = "Incorrect Password";
              }
            

            return PartialView(customer);
        }

        [HttpGet]
        public async Task<IActionResult> MoneyTransfer()
        {
           return View();
        }

        [HttpPost]
        public async Task<IActionResult> MoneyTransfer(Transaction transaction,long accountNumber)
        {
          
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));


                transaction.sender = accountNumber;
                transaction.details = $"New Transaction Of Type Transfer From => {JsonConvert.SerializeObject(transaction.sender)} To => {JsonConvert.SerializeObject(transaction.receiver)}, An Amount Of =>" +
                    $" {JsonConvert.SerializeObject(transaction.amount)} birr On Date {JsonConvert.SerializeObject(transaction.dateTime)}";

                HttpResponseMessage response = await client.PostAsJsonAsync(baseUrl + "api/transactions/transfer/", transaction);


                if (response.IsSuccessStatusCode)
                {
                    ViewBag.msg = "Transaction Successfull";

                }


                else
                {
                    ViewBag.msg = "OOPS........Something Went Wrong, Please Try Again";
                }
                Console.WriteLine("GGGGGGGGGGGGGGGGGGGGGGGGG" + response.Content.ReadAsStringAsync());
            }

            return View(transaction);
        }

    }
}
