using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Borsa.Model
{
    public class Account
    {
        public int AccountId { get; set; }
        public string AccountName { get; set; }
        public decimal TotalAmount { get; set; }
        public MoneyType MoneyType { get; set; }
        public bool DefaultAccount { get; set; }
        public List<AccountTransaction> AccountTransactions { get; set; }
    }

    public enum MoneyType
    {
        TRY = 1,
        USD = 2,
        EUR = 3
    }
}
