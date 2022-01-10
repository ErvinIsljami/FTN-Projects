using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.SHES_Components
{
    public class Battery
    {
        public string ID { get; set; }
        public double MaxPower { get; set; }
        public double Capacity { get; set; }

        public Battery(double maxPower, double capacity, string iD)
        {
            MaxPower = maxPower;
            Capacity = capacity;
            ID = iD;
        }

        public Battery()
        {

        }
    }
}
