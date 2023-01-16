using BankManagementSystemApi.Models;
using BankManagementSystemApi.Repository;
using BankManagementSystemApi.Services;
using Microsoft.Data.SqlClient;

namespace BankManagementSystemApi.Implementations
{
    public class CustomerImplementation : ICustomer
    {
        private readonly AppDbContext context;
        string connectionString = "data source=DESKTOP-CO5U0RI; database=Bank Management System; integrated security=SSPI; encrypt=false";

        public CustomerImplementation(AppDbContext context)
        {
            this.context = context;
        }


        public Customer AddCustomer(Customer customer)
        {
            long lastAdded = 0;
            if(context.Customers.Any(x=>x.email == customer.email))
            {
                throw new ApplicationException("An account already exists with this email address");
            }
            else
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "select max(accountNumber) from Customers;";
                    
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        lastAdded = (long)command.ExecuteScalar();
                    }
                }
               // long accountNum=customer.accountNumber;
                customer.accountNumber = lastAdded+1;
                context.Customers.Add(customer);
                context.SaveChanges();
                return customer;
            }
           
        }

        public void DeleteCustomer(int id)
        {
            var customer = context.Customers.Find(id);

            if (customer != null)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "delete from aspnetusers where email='" + customer.email + "';";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        var execute = command.ExecuteNonQuery();
                    }
                }
                context.Customers.Remove(customer);
                context.SaveChanges();
            }
        }

        public List<Customer> GetAllCustomers()
        {
            return context.Customers.ToList();
        }

        public Customer GetCustomerByAccountNumber(long accountNumber)
        {
            var customer = context.Customers.FirstOrDefault(x => x.accountNumber == accountNumber);
            return customer;
        }

        public Customer GetCustomerById(int id)
        {
            var customer = context.Customers.Find(id);
            return customer;
        }

        public Customer UpdateCustomer(Customer customer)
        {
            var existingCustomer = context.Customers.Find(customer.id);
            if(existingCustomer != null)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "Update aspnetusers set firstName='" + customer.firstName + "',lastName='" + customer.lastName + "',phoneNumber='" + customer.phoneNumber + "'," +
                        "email='" + customer.email + "',username='" + customer.email + "',normalizedusername='" + customer.email.ToUpper() + "',normalizedemail='" + customer.email.ToUpper() + "'" +
                        " where email='" + existingCustomer.email + "';";


                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        var execute = command.ExecuteNonQuery();
                    }
                }


                existingCustomer.picture=customer.picture;
                existingCustomer.firstName=customer.firstName;
                existingCustomer.lastName=customer.lastName;
                existingCustomer.email=customer.email;
                existingCustomer.phoneNumber = customer.phoneNumber;
                existingCustomer.city = customer.city;
                existingCustomer.address = customer.address;
                existingCustomer.accountType=customer.accountType;
                existingCustomer.balance = customer.balance;
                existingCustomer.birthDate=customer.birthDate;
                context.Customers.Update(existingCustomer);
                context.SaveChanges();
            }
            return customer;
        }
    }
}
