using BankManagementSystemApi.Models;

namespace BankManagementSystemApi.Services
{
    public interface ICustomerServiceRepresentative
    {
        List<CustomerServiceRepresentative> GetAllCustomerServiceRepresentatives();
        CustomerServiceRepresentative GetCustomerServiceRepresentativeById(int id);
        CustomerServiceRepresentative AddCustomerServiceRepresentative(CustomerServiceRepresentative representative);
        CustomerServiceRepresentative UpdateCustomerServiceRepresentative(CustomerServiceRepresentative representative);
        void DeleteCustomerServiceRepresentative(int id);
    }
}
