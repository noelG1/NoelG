using BankManagementSystemApi.Models;
using BankManagementSystemApi.Repository;
using BankManagementSystemApi.Services;
using BankManagementSystemIdentity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Microsoft.Win32;

namespace BankManagementSystemApi.Implementations
{
    public class CustomerServiceRepresentativeImplementation : ICustomerServiceRepresentative
    {
        private readonly AppDbContext context;
        string connectionString = "data source=DESKTOP-CO5U0RI; database=Bank Management System; integrated security=SSPI; encrypt=false";

        public CustomerServiceRepresentativeImplementation(AppDbContext context)
        {
            this.context = context;
           
        }
        public CustomerServiceRepresentative AddCustomerServiceRepresentative(CustomerServiceRepresentative representative)
        {
            if(context.CustomerServiceRepresentatives.Any(x=>x.email == representative.email))
            {
                throw new ApplicationException("An account already exists with this email address");
            }
            else
            {
              /*  string accountNum=customer.accountNumber;
                var lastAccountNumber = context.Customers.OrderByDescending(x=>x.registerDate).Take(1).FirstOrDefault();
                int lastAccountNumberInt = lastAccountNumber;
              
                int nextAccountNumber = lastAccountNumberInt++;
                customer.accountNumber = nextAccountNumber.ToString();*/
             context.CustomerServiceRepresentatives.Add(representative);
             context.SaveChanges();
             return representative;
            }
           
        }

        public void DeleteCustomerServiceRepresentative(int id)
        {
            var representative = context.CustomerServiceRepresentatives.Find(id);

            if(representative!=null)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "delete from aspnetusers where email='"+representative.email+"';";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        var execute = command.ExecuteNonQuery();
                    }
                }
             context.CustomerServiceRepresentatives.Remove(representative);
             context.SaveChanges();
            }


            
        }

        public List<CustomerServiceRepresentative> GetAllCustomerServiceRepresentatives()
        {
            return context.CustomerServiceRepresentatives.ToList();
        }


        public CustomerServiceRepresentative GetCustomerServiceRepresentativeById(int id)
        {
            var representative = context.CustomerServiceRepresentatives.Find(id);
            return representative;
        }

        public CustomerServiceRepresentative UpdateCustomerServiceRepresentative(CustomerServiceRepresentative representative)
        {          
            var existingRepresentative = context.CustomerServiceRepresentatives.Find(representative.id);
            if (existingRepresentative != null)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "Update aspnetusers set firstName='" + representative.firstName + "',lastName='" + representative.lastName + "',phoneNumber='" + representative.phoneNumber + "'," +
                        "email='" + representative.email + "',username='" + representative.email + "',normalizedusername='" + representative.email.ToUpper() + "',normalizedemail='" + representative.email.ToUpper() + "'" +
                        " where email='" + existingRepresentative.email +"';";


                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        var execute = command.ExecuteNonQuery();
                    }
                }
           
                existingRepresentative.picture=representative.picture;
                existingRepresentative.firstName=representative.firstName;
                existingRepresentative.lastName=representative.lastName;
                existingRepresentative.phoneNumber=representative.phoneNumber;
                existingRepresentative.email=representative.email;
                existingRepresentative.birthDate=representative.birthDate;
                context.CustomerServiceRepresentatives.Update(existingRepresentative);
                context.SaveChanges();

            }
            return representative;
        }
    }
}
