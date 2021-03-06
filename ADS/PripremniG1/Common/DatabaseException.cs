using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [DataContract]
    public class DatabaseException
    {
        string reason;

        public DatabaseException()
        {
        }

        public DatabaseException(string reason)
        {
            Reason = reason;
        }
        [DataMember]
        public string Reason { get => reason; set => reason = value; }
    }
}
