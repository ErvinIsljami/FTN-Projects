using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Common.Model
{
    [DataContract]
    public class Setpoint
    {
        public Setpoint(double value, DateTime date, string id)
        {
            Value = value;
            Date = date;
            GenId = id;
        }
        [DataMember]
        public double Value { get; set; }
        [DataMember]
        public DateTime Date { get; set; }
        [DataMember]
        public string GenId { get; set; }
    }
}
