using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;

namespace CertManager
{
	public class DigitalSignature
	{
		public static byte[] Create(string message, string hashAlgorithm, X509Certificate2 certificate)
		{
            RSACryptoServiceProvider csp = null;

            /// hash the message using SHA-1 (assume that "hashAlgorithm" is SHA-1)
            SHA1Managed sha1 = new SHA1Managed();


            /// Use RSACryptoServiceProvider support to create a signature using a previously created hash value
            byte[] signature = new byte[] { };
            return signature;
        }


		public static bool Verify(string message, string hashAlgorithm, byte[] signature, X509Certificate2 certificate)
		{
            RSACryptoServiceProvider csp = null;

            /// hash the message using SHA-1 (assume that "hashAlgorithm" is SHA-1)
            SHA1Managed sha1 = new SHA1Managed();


            /// Use RSACryptoServiceProvider support to compare two - hash value from signature and newly created hash value
            bool isValid = false;
            return isValid;
        }
	}
}