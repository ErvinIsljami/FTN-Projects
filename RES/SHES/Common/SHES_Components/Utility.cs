using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.SHES_Components
{
    public class Utility
    {
        public double ExchangePower { get; set; }
        public double Price { get; set; }

        public Utility()
        {
            ExchangePower = 0;
        }

        public Utility(double price)
        {
            ExchangePower = 0;
            Price = price;
        }
    }
}
