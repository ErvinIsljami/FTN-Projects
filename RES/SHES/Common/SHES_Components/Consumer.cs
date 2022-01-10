using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.SHES_Components
{
    public class Consumer
    {
        public string ID { get; set; }
        public double Consumption { get; set; }

        public Consumer(double consumption, string iD)
        {
            Consumption = consumption;
            ID = iD;
        }

        public Consumer()
        {

        }
    }
}
