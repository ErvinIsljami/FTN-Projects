using CertManager;
using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Server.Services
{
    public static class ProxyLog
    {
        public static ILogService Proxy;

        static ProxyLog()
        {


            string logCNT = "loger"; 
            string jaCNT = "server"; 
            NetTcpBinding binding = new NetTcpBinding();
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;
            X509Certificate2 logCerv = CertManager.CertManager.GetCertificateFromStorage(StoreName.TrustedPeople, StoreLocation.LocalMachine, logCNT);
            EndpointAddress address = new EndpointAddress(new Uri("net.tcp://localhost:10200/LogService"), new X509CertificateEndpointIdentity(logCerv));
            ChannelFactory<ILogService> channel = new ChannelFactory<ILogService>(binding, address);

            
            channel.Credentials.ServiceCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;
            channel.Credentials.ClientCertificate.Certificate = CertManager.CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, jaCNT);

            Proxy = channel.CreateChannel();
        }
    }
}
