using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [DataContract]
    public class INVERT_Cryptography : ICryptographyInterface
    {
        public override byte[] EncryptData(string data)
        {
            byte[] bytes = System.Text.Encoding.Unicode.GetBytes(data);
            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] ^= 0b10101010;
            }
            return bytes;
        }

        public override string DecryptData(byte[] bytes)
        {
            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] ^= 0b10101010;
            }
            string convertedToString = System.Text.Encoding.Unicode.GetString(bytes);

            return convertedToString;
        }
    }
}
