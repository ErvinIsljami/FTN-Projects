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

namespace BankingService
{
    public class CreditSectorClient : ChannelFactory<ICreditService>, ICreditService, IDisposable
    {
        private ICreditService factory;
        public CreditSectorClient(NetTcpBinding binding, EndpointAddress address) : base(binding, address)
        {
            string cltCertCN = "Bank";

            this.Credentials.ServiceCertificate.Authentication.CertificateValidationMode = System.ServiceModel.Security.X509CertificateValidationMode.Custom;
            this.Credentials.ServiceCertificate.Authentication.CustomCertificateValidator = new ClientCertValidator();
            this.Credentials.ServiceCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;
            this.Credentials.ClientCertificate.Certificate = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, cltCertCN);

            factory = this.CreateChannel();

        }

        public void ApplyForCredit(double amount, string userName)
        {
            factory.ApplyForCredit(amount, userName);
        }

        public void ApproveCredit(string userName)
        {
            factory.ApproveCredit(userName);
        }

        public void DenyCredit(string userName)
        {
            factory.DenyCredit(userName);
        }

        public void Dispose()
        {
            if (factory != null)
            {
                factory = null;
            }

            this.Close();
        }

        public List<Credit> GetAllCreditRequests()
        {
            return factory.GetAllCreditRequests();
        }
    }
}
