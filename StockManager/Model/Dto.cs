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
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
