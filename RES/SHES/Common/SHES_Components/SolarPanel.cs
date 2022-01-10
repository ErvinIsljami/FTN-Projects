using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.SHES_Components
{
    public class SolarPanel
    {
        public string ID { get; set; }
        public double MaxPower { get; set; }

        public SolarPanel(double maxPower, string id)
        {
            ID = id;
            MaxPower = maxPower;
        }

        public SolarPanel()
        {

        }
    }
}
