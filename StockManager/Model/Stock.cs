using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Borsa.Model
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
