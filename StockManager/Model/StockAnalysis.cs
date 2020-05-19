using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockManager.Model
{
    public class StockAnalysisTransaction
    {
        public int StockTransactionId { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Value { get { return Amount * UnitPrice; } }
        public decimal Const { get { return Value / 1000 * 2; } }
        public TransactionType TransactionType { get; set; }
    }

    public class StockAnalysis
    {
        public StockAnalysis()
        {
            StockTransactions = new List<StockAnalysisTransaction>();
        }

        public string StockCode { get; set; }
        public decimal Gain { get { return TotalAmount == 0 ? (TotalValue - TotalConst) : 0; } }
        public decimal TotalAmount { get { return StockTransactions.Sum(c => c.Amount * (c.TransactionType == TransactionType.Sell ? -1 : 1)); } }
        public decimal TotalBuyAmount { get { return StockTransactions.Where(c => c.TransactionType == TransactionType.Buy).Sum(c => c.Amount); } }
        public decimal TotalSellAmount { get { return StockTransactions.Where(c => c.TransactionType == TransactionType.Sell).Sum(c => c.Amount); } }
        public decimal TotalConst { get { return StockTransactions.Sum(c => c.Const); } }
        public decimal BuyPrice
        {
            get
            {
                if (!StockTransactions.Any(c => c.TransactionType == TransactionType.Buy))
                    return 0;
                return StockTransactions.Where(c => c.TransactionType == TransactionType.Buy).Sum(c => c.UnitPrice * c.Amount) / StockTransactions.Where(c => c.TransactionType == TransactionType.Buy).Sum(c => c.Amount);
            }
        }
        public decimal SellPrice
        {
            get
            {
                if (!StockTransactions.Any(c => c.TransactionType == TransactionType.Sell))
                    return 0;
                return StockTransactions.Where(c => c.TransactionType == TransactionType.Sell).Sum(c => c.UnitPrice * c.Amount) / StockTransactions.Where(c => c.TransactionType == TransactionType.Sell).Sum(c => c.Amount);
            }
        }
        public DateTime FirstTransactionDate
        {
            get
            {
                return StockTransactions.OrderBy(c => c.Date).First().Date;
            }
        }
        public DateTime LastTransactionDate
        {
            get
            {
                return StockTransactions.OrderByDescending(c => c.Date).First().Date;
            }
        }
        public decimal TotalValue { get { return StockTransactions.Sum(c => c.Value * (c.TransactionType == TransactionType.Buy ? -1 : 1)); } }
        public List<StockAnalysisTransaction> StockTransactions { get; set; }
    }
}
