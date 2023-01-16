using BankManagementSystemApi.Models;
using BankManagementSystemApi.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BankManagementSystemApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
         private readonly ICustomer customer;
        private readonly ICustomer _customer;      
        public CustomersController(ICustomer customer)
        {
            _customer = customer;
        }
        // GET: api/<CustomersController>
        [HttpGet]
        public IActionResult GetAll()
        {
            var customers = _customer.GetAllCustomers();
            return Ok(customers);
        }

       // GET api/<CustomersController>/5
         [HttpGet("GetSingleById/{id}")]
        public IActionResult GetSingleById(int id)
        {
            var customer = _customer.GetCustomerById(id);
            if (customer != null)
            {
                return Ok(customer);
            }
            return NotFound("Not found");
        }

        [HttpGet("GetSingleByAccountNumber/{accountNumber}")]
       // [Route("GetSingleByAccountNumber")]
        public IActionResult GetSingleByAccountNumber(long accountNumber)
        {
            var customer = _customer.GetCustomerByAccountNumber(accountNumber);
            if (customer != null)                                                                                                                                         
            {
                return Ok(customer);
            }
            return NotFound("Not found");
        }   



        // POST api/<CustomersController>
        [HttpPost]
        public IActionResult Post(Customer customer)
        {
            _customer.AddCustomer(customer);
            return Ok(customer);
        }

        // PUT api/<CustomersController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id,Customer customer)
        {
            var existingCustomer= _customer.GetCustomerById(id);
            if(existingCustomer != null)
            {
                customer.id = existingCustomer.id;
                _customer.UpdateCustomer(customer);
                return Ok(customer);
            }
            return NotFound("Customer not found");
        }

        // DELETE api/<CustomersController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            var customer=_customer.GetCustomerById(id);
            if(customer != null)
            {
                _customer.DeleteCustomer(id);
            }
        }
    }
}
