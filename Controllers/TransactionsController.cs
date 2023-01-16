using BankManagementSystemApi.Models;
using BankManagementSystemApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace BankManagementSystemApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransaction transaction;
        private readonly ITransaction _transaction;
        public TransactionsController(ITransaction transaction)
        {
            _transaction = transaction;
        }

        [HttpGet("{date}")]
        public IActionResult GetAllTransactionsByDate(DateTime date)
        {
            var transaction = _transaction.GetTransactionByDate(date);
            return Ok(transaction);
        }

        [HttpGet]
        public IActionResult GetAllTransactionsOfToday()
        {
            var transaction = _transaction.GetAllNewTransactions();
            return Ok(transaction);
        }

        [HttpPost]
        [Route("Deposit")]
        public IActionResult Deposit(Transaction transaction)
        {
            var transact = _transaction.MakeDeposit(transaction);
            return Ok(transact);
        }

        [HttpPost]
        [Route("Withdraw")]
        public IActionResult Withdraw(Transaction transaction)
        {
            var transact = _transaction.MakeWithdrawal(transaction);
            return Ok(transact);
        }
     
        [HttpPost]
        [Route("Transfer")]
        public IActionResult Transfer(Transaction transaction)
        {
            var transact = _transaction.TransferFromAccountToAccount(transaction);
            return Ok(transact);
        }
    }
}
