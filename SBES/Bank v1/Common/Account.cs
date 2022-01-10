using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class Account
    {
        public string User { get; set; }
        public double Money { get; set; }
        public bool IsAccountActive { get; set; }

        public Account()
        {

        }
        public Account(string user, double money)
        {
            User = user;
            Money = money;

        }
    }
}
