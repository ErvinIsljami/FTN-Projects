using System;
using System.Security.Cryptography.X509Certificates;
using System.Security.Permissions;
using System.Security.Principal;
using System.ServiceModel;
using System.Threading;
using BankServiceApp.AccountStorage;
using BankServiceApp.Arbitration;
using Common;
using Common.CertificateManager;
using Common.UserData;

namespace BankServiceApp.Replication
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Single, IncludeExceptionDetailInFaults = true)]
    public class ReplicatorService : IReplicator
    {
        private readonly IArbitrationServiceProvider _arbitrationServiceProvider;
        private readonly ICache _bankCache;

        public ReplicatorService()
        {
            _arbitrationServiceProvider = ServiceLocator.GetInstance<IArbitrationServiceProvider>();
            _bankCache = ServiceLocator.GetInstance<ICache>();
        }

        #region IReplicator

        [PrincipalPermission(SecurityAction.Demand, Authenticated = true, Role = "BankServices")]
        public void ReplicateData(IReplicationItem replicationData)
        {
            var principal = Thread.CurrentPrincipal;

            if ((replicationData.Type & ReplicationType.UserData) != 0) ReplicateCacheData(replicationData, principal);

            if ((replicationData.Type & ReplicationType.CertificateData) != 0)
            {
                if ((replicationData.Type & ReplicationType.RevokeCertificate) == 0)
                    RevokeOldAndPlaceNewCertificate(replicationData.Certificate);
                else
                    RevokeOldCertificate(replicationData.Certificate);
            }
        }

        private static void RevokeOldCertificate(X509Certificate2 certificate)
        {
            Console.WriteLine($"Revoking old certificate for {certificate.Subject}.");
            using (var store = new X509Store(StoreName.My, StoreLocation.LocalMachine))
            {
                store.Open(OpenFlags.MaxAllowed);
                try
                {
                    var foundCert = store.Certificates.Find(X509FindType.FindBySubjectDistinguishedName,
                        certificate.SubjectName, true)?[0];

                    if (foundCert != null) store.Remove(certificate);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Failed to find old certificate.");
                }

                store.Close();
            }
        }

        private static void RevokeOldAndPlaceNewCertificate(X509Certificate2 certificate)
        {
            Console.WriteLine($"Revoking old and placing new certificate for {certificate.Subject}.");
            using (var store = new X509Store(StoreName.My, StoreLocation.LocalMachine))
            {
                store.Open(OpenFlags.ReadWrite);
                X509Certificate2 foundCert = null;
                try
                {
                    foundCert = CertificateManager.Instance.GetCertificateFromStore(
                        StoreLocation.LocalMachine,
                        StoreName.My,
                        certificate.Subject);
                }
                catch
                {
                    foundCert = null;
                }

                if (foundCert == null)
                {
                    Console.WriteLine("Old certificate not found placing new in store.");
                    store.Add(certificate);
                }
                else if (foundCert?.SerialNumber != certificate?.SerialNumber)
                {
                    store.Remove(foundCert ?? throw new NullReferenceException(nameof(foundCert)));
                    store.Add(certificate);
                }

                store.Close();
            }

            Console.WriteLine("Done replicating certificate.");
        }

        /// <summary>
        ///     Replicate client account cache data.
        /// </summary>
        /// <param name="replicationData"> Replication data. </param>
        /// <param name="principal"> The bank service user replicating data. </param>
        private void ReplicateCacheData(IReplicationItem replicationData, IPrincipal principal)
        {
            Console.WriteLine($"Replicating cache data from {principal.Identity.Name}");
            var client = BankCache.GetClientFromCache(_bankCache, replicationData.Client.Name);
            client.Pin = replicationData.Client.Pin;
            client.Account = new Account(replicationData.Client.Account.Balance);
            _bankCache.StoreData();
        }

        [PrincipalPermission(SecurityAction.Demand, Authenticated = true, Role = "BankServices")]
        public ServiceState CheckState()
        {
            return _arbitrationServiceProvider.State;
        }

        #endregion
    }
}