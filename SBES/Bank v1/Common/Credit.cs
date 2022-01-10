using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class Credit
    {
        public string User { get; set; }
        public double CreditAmount { get; set; }
        public bool IsCreditAllowed { get; set; }

        public Credit(string user, double creditAmmount)
        {
            User = user;
            IsCreditAllowed = false;
            CreditAmount = creditAmmount;
        }

        public Credit()
        {
            User = "";
            IsCreditAllowed = false;
            CreditAmount = 0;
        }
    }
}
