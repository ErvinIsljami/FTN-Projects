using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CloudCompute
{
    // Only thing interesting for us is the number of instances
    [Serializable]
    public class ConfigData
    {
        private int _instances;


        public ConfigData()
        {

        }

        [XmlElement]
        public int Instances
        {
            get { return _instances; }
            set { _instances = value; }
        }
    }
}
