using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Security;
using System.Text;
using System.Threading.Tasks;
using Manager;

namespace AccountSector
{
    class Program
    {
        static void Main(string[] args)
        {

            List<Account> list = new List<Account>() { new Account("test", 10000) { IsAccountActive = true} };
            XmlReaderWriter xmlReaderWriter = new XmlReaderWriter();
            xmlReaderWriter.SerializeObject<List<Account>>(list, "../../../Resources/users.xml");

            NetTcpBinding binding = new NetTcpBinding();
            string address = "net.tcp://localhost:9998/AccountService";
            ServiceHost host = new ServiceHost(typeof(AccountService));

            string srvCertCN = "Sector";

            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;

            host.AddServiceEndpoint(typeof(IAccountService), binding, address);

            ///Custom validation mode enables creation of a custom validator - CustomCertificateValidator
            host.Credentials.ClientCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.Custom;
            host.Credentials.ClientCertificate.Authentication.CustomCertificateValidator = new ServiceCertValidator();

            ///If CA doesn't have a CRL associated, WCF blocks every client because it cannot be validated
            host.Credentials.ClientCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;

            ///Set appropriate service's certificate on the host. Use CertManager class to obtain the certificate based on the "srvCertCN"
            host.Credentials.ServiceCertificate.Certificate = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, srvCertCN);


            host.Open();
            Console.WriteLine("WCFService is opened. Press <enter> to finish...");
            Console.ReadLine();
            host.Close();
        }
    }
}
