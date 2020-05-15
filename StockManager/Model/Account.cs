using System.Collections.Generic;

namespace StockManager.Model
{
    public class Account
    {
        public int AccountId { get; set; }
        public string AccountName { get; set; }
        public decimal TotalAmount { get; set; }
        public MoneyType MoneyType { get; set; }
        public bool DefaultAccount { get; set; }
        public List<AccountTransaction> AccountTransactions { get; set; }
    }

    public enum MoneyType
    {
        TRY = 1,
        USD = 2,
        EUR = 3
    }
}
