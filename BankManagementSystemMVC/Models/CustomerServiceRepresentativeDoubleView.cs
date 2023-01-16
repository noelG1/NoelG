using BankManagementSystemApi.Models;
using BankManagementSystemMvc.Models;

namespace BankManagementSystemMVC.Models
{
    public class CustomerServiceRepresentativeDoubleView
    {
        public CustomerServiceRepresentative? representative { get; set; }
        public IEnumerable<CustomerServiceRepresentative>? representatives { get; set; }
        public AddCustomerServiceRepresentativeViewModel? viewModel { get; set; }
    }
}
