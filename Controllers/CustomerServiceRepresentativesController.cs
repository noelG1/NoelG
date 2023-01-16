using BankManagementSystemApi.Models;
using BankManagementSystemApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BankManagementSystemApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerServiceRepresentativesController : ControllerBase
    {
        private readonly ICustomerServiceRepresentative serviceRepresentative;

        public CustomerServiceRepresentativesController(ICustomerServiceRepresentative serviceRepresentative)
        {
            this.serviceRepresentative = serviceRepresentative;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var representatives = serviceRepresentative.GetAllCustomerServiceRepresentatives();
            return Ok(representatives);
        }

        // GET api/<CustomersController>/5
        [HttpGet("{id}")]
        public IActionResult GetSingleById(int id)
        {
            var representative = serviceRepresentative.GetCustomerServiceRepresentativeById(id);
            if (representative != null)
            {
                return Ok(representative);
            }
            return NotFound("Not found");
        }

       
        // POST api/<CustomersController>
        [HttpPost]
        public IActionResult Post(CustomerServiceRepresentative representative)
        {
            serviceRepresentative.AddCustomerServiceRepresentative(representative);
            return Ok(representative);
        }

        // PUT api/<CustomersController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, CustomerServiceRepresentative representative)
        {
            var existingRepresentative = serviceRepresentative.GetCustomerServiceRepresentativeById(id);
            if (existingRepresentative != null)
            {
                representative.id = existingRepresentative.id;
               serviceRepresentative.UpdateCustomerServiceRepresentative(representative);
               return Ok(representative);
            }
            return NotFound("Customer Service Representative not found");
        }

        // DELETE api/<CustomersController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            var representative = serviceRepresentative.GetCustomerServiceRepresentativeById(id);
            if (representative != null)
            {
                serviceRepresentative.DeleteCustomerServiceRepresentative(id);
            }
        }
    }
}
