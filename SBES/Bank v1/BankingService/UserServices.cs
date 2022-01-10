using Common;
using Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Permissions;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BankingService
{
    public class UserServices : IUserService
    {
        private ICreditService creditServiceProxy;
        private IAccountService accountServiceProxy;
        public UserServices()
        {
            string servCertN = "Sector";

            string addressAccount = "net.tcp://localhost:9998/AccountService";
            NetTcpBinding binding = new NetTcpBinding();
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;
            X509Certificate2 srvCert = CertManager.GetCertificateFromStorage(StoreName.TrustedPeople, StoreLocation.LocalMachine, servCertN); 
            EndpointAddress adr = new EndpointAddress(new Uri(addressAccount), new X509CertificateEndpointIdentity(srvCert));
            accountServiceProxy = new AccountSectorClient(binding, adr);


            string addressCredit = "net.tcp://localhost:9997/CreditService";
            NetTcpBinding binding2 = new NetTcpBinding();
            binding2.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;
            X509Certificate2 srvCert2 = CertManager.GetCertificateFromStorage(StoreName.TrustedPeople, StoreLocation.LocalMachine, servCertN);
            EndpointAddress adr2 = new EndpointAddress(new Uri(addressCredit), new X509CertificateEndpointIdentity(srvCert2));
            creditServiceProxy = new CreditSectorClient(binding2, adr2);
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "CreateRequest")]
        public void ApplyForCredit(string userName, double amount)
        {
            creditServiceProxy.ApplyForCredit(amount, userName);
            Audit a = new Audit();
            a.AuditEvent($"{userName} applied for {amount} credit.");
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "ExecuteTransaction")]
        public void ExecuteTransaction(string userName, double amount)
        {
            accountServiceProxy.ExecuteTransaction(amount, userName);
            Audit a = new Audit();
            a.AuditEvent($"{userName} executed transaction of {amount} amount.");
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "CreateRequest")]
        public void MakeNewAccount(string userName)
        {
            accountServiceProxy.MakeNewAccount(userName);
            Audit a = new Audit();
            a.AuditEvent($"New account created. Username: {userName}.");
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "ApproveRequest")]
        public void ApproveAccount(string userName)
        {
            accountServiceProxy.ApproveAccount(userName);
            Audit a = new Audit();
            a.AuditEvent($"New account approved. Username: {userName}.");
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "ApproveRequest")]
        public void ApproveCredit(string userName)
        {
            creditServiceProxy.ApproveCredit(userName);
            Audit a = new Audit();
            a.AuditEvent($"New account approved. Username: {userName}.");
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "ApproveRequest")]
        public List<Account> GetAllAccountRequests()
        {
            return accountServiceProxy.GetAllAccountRequests().Where(x => x.IsAccountActive == false).ToList();
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "ApproveRequest")]
        public List<Credit> GetAllCreditRequests()
        {
            return creditServiceProxy.GetAllCreditRequests().Where(x => x.IsCreditAllowed == false).ToList();
        }
        [PrincipalPermission(SecurityAction.Demand, Role = "ApproveRequest")]
        public void DenyCredit(string userName)
        {
            creditServiceProxy.DenyCredit(userName);
        }
        [PrincipalPermission(SecurityAction.Demand, Role = "ApproveRequest")]
        public void DenyAccount(string userName)
        {
            accountServiceProxy.DenyAccount(userName);
        }
    }
}
