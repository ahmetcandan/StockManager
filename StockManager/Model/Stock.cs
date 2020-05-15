using System;

namespace StockManager.Model
{
    public class Stock
    {
        public string StockCode { get; set; }
        public string Name { get; set; }
        public decimal HighestValueOfDay { get; set; }
        public decimal LowestValueOfDay { get; set; }
        public decimal Value { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
