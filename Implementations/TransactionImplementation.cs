using BankManagementSystemApi.Models;
using BankManagementSystemApi.Repository;
using BankManagementSystemApi.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BankManagementSystemApi.Implementations
{
    public class TransactionImplementation : ITransaction
    {
        private readonly AppDbContext context;
        private readonly ICustomer customer;

        public TransactionImplementation(AppDbContext context,ICustomer customer)
        {
            this.context = context;
            this.customer = customer;
        }
        public Transaction AddTransaction(Transaction transaction)
        {
            context.Transactions.Add(transaction);
            context.SaveChanges();
            return transaction;
        }

        public List<Transaction> GetTransactionByDate(DateTime date)
        {
            var transaction = context.Transactions.Where(x => x.dateTime.Date == date).ToList();           
              return transaction;           
            
        }

        public List<Transaction> GetAllNewTransactions()
        {
            var transaction=context.Transactions.Where(x=>x.dateTime.Date==DateTime.Today).ToList();
            return transaction;
        }



        public Transaction MakeDeposit(Transaction transaction)
        {
           
            Customer account;
           account=customer.GetCustomerByAccountNumber(transaction.receiver);

            account.balance += transaction.amount;
            if (context.Entry(account).State == Microsoft.EntityFrameworkCore.EntityState.Modified)
            {
                transaction.status=TransactionStatus.Success;
            }
            else
            {
                transaction.status = TransactionStatus.Failed;
            }

            transaction.type = TransactionType.Deposit;
           // transaction.amount = amount;
           // transaction.receiver = accountNumber;
            transaction.sender = transaction.receiver;
            transaction.dateTime = DateTime.Now;
            transaction.details = $"New Transaction Of Type Deposit To => {JsonConvert.SerializeObject(transaction.receiver)} An Amount Of =>" +
                $" {JsonConvert.SerializeObject(transaction.amount)} On Date {JsonConvert.SerializeObject(transaction.dateTime)}";
            context.Transactions.Add(transaction);
            context.SaveChanges();
            return transaction;
        }



        public Transaction MakeWithdrawal(Transaction transaction)
        {
           // Transaction transaction = new Transaction();
            Customer account;
            account = customer.GetCustomerByAccountNumber(transaction.sender);
            if (account.balance > transaction.amount)
            {
              account.balance -= transaction.amount;
            }
            else
            {
                Console.WriteLine("Not Enough Funds To Withdraw The Requested Amount");
            }

            if (context.Entry(account).State == Microsoft.EntityFrameworkCore.EntityState.Modified)
            {
                transaction.status = TransactionStatus.Success;
            }
            else
            {
                transaction.status = TransactionStatus.Failed;
            }

            transaction.type = TransactionType.Withdraw;
          //  transaction.amount = amount;
            transaction.receiver = transaction.sender;
           // transaction.sender = accountNumber;
            transaction.dateTime = DateTime.Now;
            transaction.details = $"New Transaction Of Type Withdraw From => {JsonConvert.SerializeObject(transaction.sender)} An Amount Of =>" +
                $" {JsonConvert.SerializeObject(transaction.amount)} On Date {JsonConvert.SerializeObject(transaction.dateTime)}";
            context.Transactions.Add(transaction);
            context.SaveChanges();
            return transaction;
        }



        public Transaction TransferFromAccountToAccount(Transaction transaction)
        {
          //  Transaction transaction = new Transaction();
            Customer senderAccount;
            Customer receiverAccount;
            senderAccount = customer.GetCustomerByAccountNumber(transaction.sender);
            receiverAccount = customer.GetCustomerByAccountNumber(transaction.receiver);
            if(senderAccount.balance>transaction.amount)
            { 
              senderAccount.balance -= transaction.amount;
              receiverAccount.balance += transaction.amount;
            }
            else {
                Console.WriteLine("Not Enough Funds To Transfer The Requested Amount");
            }
            if (context.Entry(senderAccount).State == Microsoft.EntityFrameworkCore.EntityState.Modified && context.Entry(receiverAccount).State == Microsoft.EntityFrameworkCore.EntityState.Modified)
            {
                transaction.status = TransactionStatus.Success;
            }
            else
            {
                transaction.status = TransactionStatus.Failed;
            }

            transaction.type = TransactionType.Deposit;
            transaction.dateTime = DateTime.Now;
            transaction.details = $"New Transaction Of Type Transfer From =>{JsonConvert.SerializeObject(transaction.sender)} To => {JsonConvert.SerializeObject(transaction.receiver)} An Amount Of =>" +
                $" {JsonConvert.SerializeObject(transaction.amount)} On Date {JsonConvert.SerializeObject(transaction.dateTime)}";
            context.Transactions.Add(transaction);
            context.SaveChanges();
            return transaction;
        }
    }
}
