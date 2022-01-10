using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Common.Model
{
    [DataContract]
    public class SetpointArray
    {
        [DataMember]
        public  List<Setpoint> Array { get; set; }

        public SetpointArray()
        {
            Array = new  List<Setpoint>();
        }
    }
}
