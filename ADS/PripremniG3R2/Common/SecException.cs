﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [DataContract]
    public class SecException
    {
        string reason;

        public SecException()
        {
        }

        public SecException(string reason)
        {
            this.reason = reason;
        }
        [DataMember]
        public string Reason { get => reason; set => reason = value; }
    }
}
