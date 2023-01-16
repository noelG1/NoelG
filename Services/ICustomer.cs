using BankManagementSystemApi.Models;

namespace BankManagementSystemApi.Services
{
    public interface ICustomer
    {
        List<Customer> GetAllCustomers();
        Customer GetCustomerById(int id);
        Customer GetCustomerByAccountNumber(long accountNumber);
        Customer AddCustomer(Customer customer);
        Customer UpdateCustomer(Customer customer);
        void DeleteCustomer(int id);
    }
}
