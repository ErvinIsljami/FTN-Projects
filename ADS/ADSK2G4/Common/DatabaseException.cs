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
        private string reason;

        public DatabaseException(string reason)
        {
            this.reason = reason;
        }

        public DatabaseException()
        {
        }

        [DataMember]
        public string Reason { get => reason; set => reason = value; }
    }
}
