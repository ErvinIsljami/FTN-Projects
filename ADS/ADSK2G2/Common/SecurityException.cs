﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [DataContract]
    public class SecurityException
    {
        string reason;

        public SecurityException(string reason)
        {
            this.reason = reason;
        }

        public SecurityException()
        {
        }
        [DataMember]
        public string Reason { get => reason; set => reason = value; }
    }
    
}
