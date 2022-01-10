using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RESProject.Classes
{
    public class CollectionDescription
    {
        public int ID { get; set; }
        public int DataSet { get; set; }
        public List<DumpingProperty> DumpingPropertyCollection { get; set; }
        public CollectionDescription()
        {
            DumpingPropertyCollection = new List<DumpingProperty>();
        }

        public CollectionDescription(int iD, int dataSet, List<DumpingProperty> dumpingPropertyCollection)
        {
            ID = iD;
            DataSet = dataSet;
            DumpingPropertyCollection = dumpingPropertyCollection;
        }

        public CollectionDescription(int iD, int dataSet)
        {
            ID = iD;
            DataSet = dataSet;
            DumpingPropertyCollection = new List<DumpingProperty>();
        }
    }
}
