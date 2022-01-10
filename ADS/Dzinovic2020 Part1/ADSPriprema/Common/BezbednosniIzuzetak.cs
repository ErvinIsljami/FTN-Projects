using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [DataContract]
    public class BezbednosniIzuzetak
    {
        [DataMember]
        public string Reason { get; set; }

        public BezbednosniIzuzetak()
        {

        }

        public BezbednosniIzuzetak(string reason)
        {
            Reason = reason;
        }
    }
}
