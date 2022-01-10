
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [DataContract]
    public class ChangeSet
    {
        public ChangeSet(double value, long timeStamp)
        {
            Value = value;
            TimeStamp = timeStamp;
        }

        public ChangeSet()
        {

        }

        [DataMember]
        public double Value { get; set; }
        [DataMember]
        public long TimeStamp { get; set; }


    }
}
