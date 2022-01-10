using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Communication
{
    public class Request
    {
        public double PowerDiff { get; set; }
        public string ComponentId { get; set; }

        public Request()
        {

        }

        public Request(double powerDiff, string componentId)
        {
            PowerDiff = powerDiff;
            ComponentId = componentId;
        }
    }
}