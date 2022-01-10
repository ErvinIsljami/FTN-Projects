using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class DataModel
    {
        public string Propertie1 { get; set; }
        public string Propertie2 { get; set; }

        public DataModel(string prop1, string prop2)
        {
            Propertie1 = prop1;
            Propertie2 = prop2;
        }

        public DataModel()
        {
        }
    }
}
