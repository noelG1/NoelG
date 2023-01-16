using Microsoft.AspNetCore.Mvc;
using System.Transactions;
using Newtonsoft.Json;
using BankManagementSystemApi.Models;
using Transaction = BankManagementSystemApi.Models.Transaction;
using BankManagementSystemApi.Services;
using BankManagementSystemMVC.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace BankManagementSystemMVC.Controllers
{
    public class TransactionsController : Controller
    {
        string baseUrl = "https://localhost:7277/";

        [HttpGet]
        public async Task<IActionResult> GetTransactionsByDate(DateTime date)
        {
            List<Transaction> transactions = new List<Transaction>(); 
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.GetAsync("api/transactions/" + date);

                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    transactions = JsonConvert.DeserializeObject<List<Transaction>>(result);                  
                }
            }
            return PartialView(transactions);
        }

        [HttpGet]
        public async Task<IActionResult> MoneyDepositAndWithdraw()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> MoneyDepositAndWithdraw(Transaction transaction,string actionChoice)
        {  
           
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                if(actionChoice=="Deposit")
                {
                   transaction.sender =transaction.receiver;
                   transaction.details = $"New Transaction Of Type Deposit To => {JsonConvert.SerializeObject(transaction.receiver)} An Amount Of =>" +
                    $" {JsonConvert.SerializeObject(transaction.amount)} birr On Date {JsonConvert.SerializeObject(transaction.dateTime)}";

                    HttpResponseMessage response =await client.PostAsJsonAsync(baseUrl + "api/transactions/deposit/",transaction);


                  if (response.IsSuccessStatusCode)
                  {
                     ViewBag.msg = "Transaction Successfull";                  
                  }


                  else
                  {
                    ViewBag.msg = "OOPS........Something Went Wrong, Please Try Again";
                  }
                  Console.WriteLine("GGGGGGGGGGGGGGGGGGGGGGGGG"+response.Content.ReadAsStringAsync());
                }

               else if (actionChoice == "Withdraw")
                {
                    transaction.sender = transaction.receiver;
                    transaction.details = $"New Transaction Of Type Withdraw From => {JsonConvert.SerializeObject(transaction.sender)} An Amount Of =>" +
                        $" {JsonConvert.SerializeObject(transaction.amount)} birr On Date {JsonConvert.SerializeObject(transaction.dateTime)}";

                    HttpResponseMessage response = await client.PostAsJsonAsync(baseUrl + "api/Transactions/Withdraw/", transaction);


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


            }

            return View(transaction);
        }




        


        [HttpGet]
        public IActionResult MoneyTransfer()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> MoneyTransfer(Transaction transaction)
        {

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            
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

        [HttpGet]
        public async Task<PartialViewResult> GetCustomerByAccountNumber(long receiver)
        {
             Transaction transaction= new Transaction();
            //  accountNumber = transaction.receiver;
            Customer customer = new Customer();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.GetAsync("api/customers/GetSingleByAccountNumber/" + receiver);

                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    customer = JsonConvert.DeserializeObject<Customer>(result);
                }
            }
            return PartialView(customer);
        }
    }
}
