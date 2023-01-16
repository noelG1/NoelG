using BankManagementSystemApi.Models;
using BankManagementSystemIdentity.Models;
using BankManagementSystemMvc.Models;
using BankManagementSystemMVC.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using BankManagementSystemApi.Services;

namespace BankManagementSystemMVC.Controllers
{
    public class ManagersController : Controller
    {

        public ManagersController(IWebHostEnvironment webHostEnvironment,UserManager<ApplicationUser>userManager,RoleManager<ApplicationRole>roleManager)
        {
            this.webHostEnvironment = webHostEnvironment;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        string baseUrl = "https://localhost:7277/";
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<ApplicationRole> roleManager;

        public IActionResult Index()
        {
            return View();
        }

        public int CountCustomers()
        {
            string query = "SELECT COUNT(*) FROM Customers";
            int count = 0;

            using (SqlConnection connection = new SqlConnection("data source=DESKTOP-CO5U0RI; database=Bank Management System; integrated security=SSPI; encrypt=false"))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    count = (int)command.ExecuteScalar();
                }
            }
            return count;

        }

        public int CountCustomerServiceRepresentatives()
        {
            string query = "SELECT COUNT(*) FROM CustomerServiceRepresentatives";
            int count = 0;

            using (SqlConnection connection = new SqlConnection("data source=DESKTOP-CO5U0RI; database=Bank Management System; integrated security=SSPI; encrypt=false"))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    count = (int)command.ExecuteScalar();
                }
            }
            return count;
        }

        [HttpGet]
        public async Task<IActionResult> GetNewTransactions()
        {
            List<Transaction> transactions = new List<Transaction>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.GetAsync("api/transactions");

                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    transactions = JsonConvert.DeserializeObject<List<Transaction>>(result);
                }
            }
            return View(transactions);
        }


        public int CountNewTransactions()
        {
            string query = "select count(*) from Transactions where dateTime > GETDate()-1";
            int count = 0;

            using (SqlConnection connection = new SqlConnection("data source=DESKTOP-CO5U0RI; database=Bank Management System; integrated security=SSPI; encrypt=false"))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    count = (int)command.ExecuteScalar();
                }
            }
            return count;            

        }

        public int CountNewCustomers()
        {
            string query = "select count(*) from Customers where registerDate > GETDate()-1";
            int count = 0;

            using (SqlConnection connection = new SqlConnection("data source=DESKTOP-CO5U0RI; database=Bank Management System; integrated security=SSPI; encrypt=false"))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    count = (int)command.ExecuteScalar();
                }
            }
            return count;

        }

        public int CountNewCustomerServiceRepresentatives()
        {
            string query = "select count(*) from CustomerServiceRepresentatives where registerDate > GETDate()-1";
            int count = 0;

            using (SqlConnection connection = new SqlConnection("data source=DESKTOP-CO5U0RI; database=Bank Management System; integrated security=SSPI; encrypt=false"))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    count = (int)command.ExecuteScalar();
                }
            }
            return count;

        }

        [HttpGet]
        public IActionResult AddBankEntity()
        {
            return PartialView();
        }


        [HttpPost]
        public async Task<IActionResult> AddBankEntity(AddEntityViewModel entity)
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    if (entity.picture != null)
                    {
                        string folder = "Images/CustomerServiceRepresentatives/";
                        string serverFolder = Path.Combine(webHostEnvironment.WebRootPath, folder);
                        if (!Directory.Exists(serverFolder))
                            Directory.CreateDirectory(serverFolder);
                        string virFilePath = folder + Guid.NewGuid().ToString() + entity.picture.FileName;
                        string physicalFilePath = Path.Combine(webHostEnvironment.WebRootPath, virFilePath);
                        entity.pictureUrl = virFilePath;


                        entity.picture.CopyToAsync(new FileStream(physicalFilePath, FileMode.Create));

                    }

                    var newRepresentative = new CustomerServiceRepresentative()
                    {
                        picture = entity.pictureUrl,
                        firstName = entity.firstName,
                        lastName = entity.lastName,
                        email = entity.email,
                        phoneNumber = entity.phoneNumber,
                        city = entity.city,
                        address = entity.address,
                        birthDate = entity.birthDate,
                        salary = entity.salary,
                        registerDate = DateTime.Now
                    };


                    var newCustomer = new Customer()
                    {
                        picture = entity.pictureUrl,
                        firstName = entity.firstName,
                        lastName = entity.lastName,
                        email = entity.email,
                        phoneNumber =entity.phoneNumber,
                        city = entity.city,
                        address = entity.address,
                        birthDate = entity.birthDate,
                        accountType = entity.accountType,
                        balance = entity.balance,
                        registerDate = DateTime.Now
                     };

                    if(entity.role=="Customer Service Representative")
                    {
                        
                        HttpResponseMessage response = await client.PostAsJsonAsync(baseUrl + "api/CustomerServiceRepresentatives/", newRepresentative);

                      if (response.IsSuccessStatusCode)
                      {
                        var user = new ApplicationUser { UserName = entity.email, firstName = entity.firstName, lastName = entity.lastName, city = entity.city, gender = entity.gender, Email = entity.email, PhoneNumber = entity.phoneNumber };
                        var result = await userManager.CreateAsync(user, entity.password);
                        var role = new ApplicationRole { Name = entity.role, roleDescription = "Customer Service Representative Officer For The Bank", creationTime = DateTime.Now };
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

                        foreach (var error in result.Errors)
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


                    else if (entity.role == "Manager")
                    {
                            var user = new ApplicationUser { UserName = entity.email, firstName = entity.firstName, lastName = entity.lastName, city = entity.city, gender = entity.gender, Email = entity.email, PhoneNumber = entity.phoneNumber };
                            var result = await userManager.CreateAsync(user, entity.password);
                            var role = new ApplicationRole { Name = entity.role, roleDescription = "Manages The Bank", creationTime = DateTime.Now };
                            var resultRole = await roleManager.CreateAsync(role);


                            if (result.Succeeded)
                            {
                                await userManager.AddToRoleAsync(user, role.Name);
                                ViewBag.msg = "Manager Registered Successfully.";
                            }
                            else
                            {
                                ViewBag.msg = "Manager Registration Was Not Successfull.";
                            }

                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError(string.Empty, error.Description);
                            }
                        
                       

                    }

                    else if (entity.role == "Customer")
                    {

                        HttpResponseMessage response = await client.PostAsJsonAsync(baseUrl + "api/Customers/", newCustomer);
                       
                        if (response.IsSuccessStatusCode)
                        {
                            var user = new ApplicationUser { UserName = entity.email, firstName = entity.firstName, lastName = entity.lastName, city = entity.city, gender = entity.gender, Email = entity.email, PhoneNumber = entity.phoneNumber };
                            var result = await userManager.CreateAsync(user, entity.password);
                            var role = new ApplicationRole { Name = entity.role, roleDescription = "Role Description", creationTime = DateTime.Now };
                            var resultRole = await roleManager.CreateAsync(role);


                            if (result.Succeeded) 
                            {
                                await userManager.AddToRoleAsync(user, role.Name);
                              
                            }

                            else
                            {
                                string query = "delete from customers where email='"+newCustomer.email+"'";

                                using (SqlConnection connection = new SqlConnection("data source=DESKTOP-CO5U0RI; database=Bank Management System; integrated security=SSPI; encrypt=false"))
                                {
                                    using (SqlCommand command = new SqlCommand(query, connection))
                                    {
                                        connection.Open();
                                        var execute = command.ExecuteNonQuery();
                                    }
                                }
                                return View(entity);
                              
                            }

                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError(string.Empty, error.Description);
                            }
                        }
                        else
                        {
                            var console = await response.Content.ReadAsStringAsync();
                            Console.WriteLine("Errorrrrrrrrrrr" + console);
                        }

                       
                    }



                }



            }

            return RedirectToAction("Index", "Managers");

        }


    }
}
