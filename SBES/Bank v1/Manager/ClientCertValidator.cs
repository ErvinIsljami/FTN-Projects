using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IdentityModel.Selectors;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.Diagnostics;

namespace Manager
{
	public class ClientCertValidator : X509CertificateValidator
	{
		/// <summary>
		/// Implementation of a custom certificate validation on the client side.
		/// Client should consider certificate valid if the given certifiate is not self-signed.
		/// If validation fails, throw an exception with an adequate message.
		/// </summary>
		/// <param name="certificate"> certificate to be validate </param>
		public override void Validate(X509Certificate2 certificate)
		{
            /// This will take clients's certificate from storage
            var name = Formatter.ParseName(WindowsIdentity.GetCurrent().Name);
            X509Certificate2 cliCert = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, name);

            if (!certificate.Issuer.Equals(cliCert.Issuer))
            {
                throw new Exception("Certificate is not from the valid issuer.");
            }
           
        }
	}
}
