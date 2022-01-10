using Common;
using Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ChatRoom
{
    public class ServerSecondaryServerChannel : IDisposable
    {
        public ISecondaryServer ProxyForward
        {
            get
            {
                string srvCertCN = "SServer";

                NetTcpBinding binding = new NetTcpBinding();
                binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;
                ChannelFactory<ISecondaryServer> factory = null;

                X509Certificate2 srvCert = CertManager.GetCertificateFromStorage(StoreName.TrustedPeople, StoreLocation.LocalMachine, srvCertCN);
                EndpointAddress address = new EndpointAddress(new Uri("net.tcp://localhost:8888/ISecondaryServer"),
                                          new X509CertificateEndpointIdentity(srvCert));

                factory = new ChannelFactory<ISecondaryServer>(binding, address);

                string cltCertCN = Formatter.ParseName(WindowsIdentity.GetCurrent().Name);

                factory.Credentials.ServiceCertificate.Authentication.CertificateValidationMode = System.ServiceModel.Security.X509CertificateValidationMode.ChainTrust;
                factory.Credentials.ServiceCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;

                factory.Credentials.ClientCertificate.Certificate = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, cltCertCN);

                return factory.CreateChannel();
            }
        }

        public void Dispose()
        {
        }
    }
}