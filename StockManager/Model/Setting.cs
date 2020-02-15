using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Borsa.Model
{
    public class Setting
    {
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public bool RememberUserName { get; set; }
        public bool RememberPassword { get; set; }
    }
}
