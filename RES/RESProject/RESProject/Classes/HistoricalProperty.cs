using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RESProject.Classes
{
    public class HistoricalProperty
    {
        public Codes Code { get; set; }
        public int HistoricalValue { get; set; }

        public HistoricalProperty()
        {

        }

        public HistoricalProperty(Codes code, int historicalValue)
        {
            Code = code;
            HistoricalValue = historicalValue;
        }
    }
}
