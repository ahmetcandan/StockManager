using System.Collections.Generic;

namespace StockManager.Model
{
    public class Setting
    {
        public Setting()
        {
            ConstRates = new List<ConstRate>();
        }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public bool RememberUserName { get; set; }
        public bool RememberPassword { get; set; }
        public string LanguageCode { get; set; }
        public List<ConstRate> ConstRates { get; set; }
    }

    public class ConstRate
    {
        public decimal StartPrice { get; set; }
        public decimal EndPrice { get; set; }
        public decimal Rate { get; set; }
    }
}
