using System;

namespace StockManager.Model
{
    public class AccountTransaction
    {
        public int AccountId { get; set; }
        public Account Account { get; set; }
        public int AccountTransactionId { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public int? StockTransactionId { get; set; }
        public StockTransaction StockTransaction { get; set; }
    }
}
