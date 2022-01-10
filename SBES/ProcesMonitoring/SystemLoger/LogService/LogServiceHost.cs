using CertManager;
using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Security;
using System.Text;
using System.Threading.Tasks;

namespace SystemLoger.LogService
{
    public class LogServiceHost
    {
        private ServiceHost svc;

        public LogServiceHost()
        {
            string address = "net.tcp://localhost:10200/LogService";

            svc = new ServiceHost(typeof(LogService));

            string srvCertCN = "loger";
            NetTcpBinding binding = new NetTcpBinding();
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;
            svc.AddServiceEndpoint(typeof(ILogService), binding, address);


            svc.Credentials.ClientCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;
            svc.Credentials.ServiceCertificate.Certificate = CertManager.CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, srvCertCN);
        }

        public void StartService()
        {
            Console.WriteLine("Starting process service...");
            svc.Open();
        }

        public void StopService()
        {
            svc.Close();
        }
    }
}
