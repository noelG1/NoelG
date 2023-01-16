using BankManagementSystemApi.Models;
using BankManagementSystemMvc.Models;

namespace BankManagementSystemMVC.Models
{
    public class CustomerDoubleView
    {
        public Customer customer { get; set; }
        public IEnumerable<Customer> customers { get; set; }
        public AddCustomerViewModel viewModel { get; set; }
    }
}
