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
    public class AccountSectorClient : ChannelFactory<IAccountService>, IAccountService, IDisposable
    {
        IAccountService factory;

        public AccountSectorClient(NetTcpBinding binding, EndpointAddress address) : base(binding, address)
        {
            string cltCertCN = "Bank";
           
            this.Credentials.ServiceCertificate.Authentication.CertificateValidationMode = System.ServiceModel.Security.X509CertificateValidationMode.Custom;
            this.Credentials.ServiceCertificate.Authentication.CustomCertificateValidator = new ClientCertValidator();
            this.Credentials.ServiceCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;
            this.Credentials.ClientCertificate.Certificate = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, cltCertCN);

            factory = this.CreateChannel();
        }

        public void ExecuteTransaction(double amount, string userName)
        {
            try
            {
                factory.ExecuteTransaction(amount, userName);
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public void MakeNewAccount(string userName)
        {
            try
            {
                factory.MakeNewAccount(userName);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void Dispose()
        {
            if (factory != null)
            {
                factory = null;
            }

            this.Close();
        }

        public List<Account> GetAllAccountRequests()
        {
            return factory.GetAllAccountRequests();
        }

        public void ApproveAccount(string userName)
        {
            factory.ApproveAccount(userName);
        }

        public void DenyAccount(string userName)
        {
            factory.DenyAccount(userName);
        }
    }
}
