using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [DataContract]
    [KnownType(typeof(AES_Cryptography))]
    [KnownType(typeof(INVERT_Cryptography))]
    public abstract class ICryptographyInterface
    {
        public abstract byte[] EncryptData(string data);

        public abstract string DecryptData(byte[] data);

        [DataMember]
        private byte[] Key { get; set; }
    }
}