using BankManagementSystemApi.Models;

namespace BankManagementSystemApi.Services
{
    public interface ITransaction
    {
        Transaction AddTransaction(Transaction transaction);
        List<Transaction>GetTransactionByDate(DateTime date);
        List<Transaction> GetAllNewTransactions();
        Transaction MakeDeposit(Transaction transaction);
        Transaction MakeWithdrawal(Transaction transaction);
        Transaction TransferFromAccountToAccount(Transaction transaction);
    }
}
