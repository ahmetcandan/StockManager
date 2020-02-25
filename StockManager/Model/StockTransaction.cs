using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockManager.Model
{
    public class StockTransaction
    {
        public int StockTransactionId { get; set; }
        public DateTime Date { get; set; }
        public string StockCode { get; set; }
        public Stock Stock { get; set; }
        public decimal Amount { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public TransactionType TransactionType { get; set; }
        public int AccountTransactionId { get; set; }
        public AccountTransaction AccountTransaction { get; set; }
    }

    public enum TransactionType
    {
        Buy = 1,
        Sell = 2
    }
}
