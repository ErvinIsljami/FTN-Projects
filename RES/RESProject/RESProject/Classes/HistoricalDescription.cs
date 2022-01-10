using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RESProject.Classes
{
    public class HistoricalDescription
    {
        public int ID { get; set; }
        public List<HistoricalProperty> HistoricalProperties { get; set; }
        public int DataSet { get; set; }

        public HistoricalDescription()
        {
            HistoricalProperties = new List<HistoricalProperty>();
        }

        public HistoricalDescription(int iD, int dataSet)
        {
            ID = iD;
            DataSet = dataSet;
            HistoricalProperties = new List<HistoricalProperty>();
        }
    }
}
