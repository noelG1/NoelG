namespace BankManagementSystemApi.Models
{
   
    public class Transaction
    {
        public Transaction()
        {
            uniqueReference=Guid.NewGuid().ToString();
        }
        public int Id { get; set; }
        public string uniqueReference { get; set; }
        public long sender { get; set; }
        public long receiver { get; set; }
        public double amount { get; set; }
        public TransactionStatus status { get; set; }
        public bool isSuccessful => status.Equals(TransactionStatus.Success);
        public TransactionType type { get; set; }
        public string details { get; set; }
        public DateTime dateTime{get;set;}




    }

    public enum TransactionStatus
    {
        Failed,
        Error,
        Success
    }
    public enum TransactionType
    {
        Deposit,
        Withdraw,
        Transfer
    }
}
