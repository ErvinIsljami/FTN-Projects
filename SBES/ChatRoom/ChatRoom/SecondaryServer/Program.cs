using ChatRoom;
using Common;
using Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.ServiceModel;
using System.ServiceModel.Security;
using System.Text;
using System.Threading.Tasks;

namespace SecondaryServer
{
    internal class Program
    {
        private static void Main(string[] args)
        {         
            ServiceHost host = new ServiceHost(typeof(SecondaryServer));
            NetTcpBinding binding = new NetTcpBinding();
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;
            
            host.Credentials.ClientCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.ChainTrust;
            host.Credentials.ClientCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;

            host.Credentials.ServiceCertificate.Certificate = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, Formatter.ParseName(WindowsIdentity.GetCurrent().Name));

            host.AddServiceEndpoint(typeof(ISecondaryServer), binding, new Uri("net.tcp://localhost:8888/ISecondaryServer"));

            host.Open();

            Console.WriteLine("Secondary Server is started.");
            Console.ReadLine();

            host.Close();
        }
    }
}