using System;
using System.Linq;

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
        public decimal Const
        {
            get
            {
                if (!Session.Entities.GetStock(StockCode).CalculateConst)
                    return 0;
                decimal totalPrice = UnitPrice * Amount;
                var constRates = Session.Entities.GetSetting().ConstRates.Where(c => totalPrice > c.StartPrice && totalPrice <= c.EndPrice);
                if (constRates.Any())
                    return totalPrice * constRates.First().Rate;
                return 0;
            }
        }
        public AccountTransaction AccountTransaction { get; set; }
    }

    public enum TransactionType
    {
        Buy = 1,
        Sell = 2
    }
}
