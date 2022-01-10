using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RESProject.Classes
{
    public class DumpingProperty
    {
        public Codes Code { get; set; }
        public double DumpingValue { get; set; }

        public DumpingProperty() { }

        public DumpingProperty(double code, double dumpingValue)
        {
            Code = (Codes)code;
            DumpingValue = dumpingValue;
        }
    }
}
