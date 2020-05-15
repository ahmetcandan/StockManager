using System.Collections.Generic;

namespace StockManager.Model
{
    public class User
    {
        public User()
        {
            Accounts = new List<Account>();
        }

        public string UserName { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public List<Account> Accounts { get; set; }
        public string LanguageCode { get; set; }
    }
}
