using StockManager.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockManager
{
    public class StockAnalysisRequest
    {
        public StockAnalysisRequest()
        {
            StockCodes = new List<string>();
        }

        public List<string> StockCodes { get; set; }
        public Period Period { get; set; }
    }

    public class StockCurrent
    {
        public string StockCode { get; set; }
        public decimal Price { get; set; }
        public DateTime UpdateDate { get; set; }
        public string StockName { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
