using System;
using System.Diagnostics;
using System.Security;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;
using BankServiceApp.AccountStorage;
using BankServiceApp.Arbitration;
using BankServiceApp.Replication;
using Common;
using Common.CertificateManager;
using Common.DataEncapsulation;
using Common.EventLogData;
using Common.ServiceContracts;
using Common.UserData;

namespace BankServiceApp.BankServices
{
    public class BankMasterCardService : IBankMasterCardService
    {
        private readonly string
            _applicationName = BankAppConfig.BankName; //System.AppDomain.CurrentDomain.FriendlyName;

        private readonly IArbitrationServiceProvider _arbitrationServiceProvider;
        private readonly ICache _bankCache;
        private readonly IReplicator _replicatorProxy;

        public BankMasterCardService()
        {
            var caCertificate = CertificateManager.Instance.GetCACertificate();
            if (caCertificate == null) throw new Exception("Certificate manager returned null for CA certificate.");

            _bankCache = ServiceLocator.GetInstance<ICache>();
            _arbitrationServiceProvider = ServiceLocator.GetInstance<IArbitrationServiceProvider>();
            _replicatorProxy = ProxyPool.GetProxy<IReplicator>();
        }

        private string GenerateRandomPin()
        {
            var newPin = "";

            var randomValues = new Random((int) DateTime.Now.Ticks);

            for (var i = 0; i < 4; ++i)
                newPin += randomValues.Next(0, 9).ToString();

            return newPin;
        }

        private string ExtractUsernameFromFullName(string fullName)
        {
            var index = fullName.LastIndexOf("\\");
            return fullName.Substring(index + 1, fullName.Length - index - 1);
        }

        private static bool RevokeCertificate(string clientName)
        {
            var success = false;

            var subject = $"CN={clientName}";

            using (var store = new X509Store(StoreName.My, StoreLocation.LocalMachine))
            {
                store.Open(OpenFlags.MaxAllowed);
                // Returns all certificates containing substring with subject name
                //Eg. clientname, clientname2, clientname3 are returned when just clientname is queried
                var certificates = store.Certificates.Find(X509FindType.FindBySubjectName, clientName, true);

                foreach (var certificate in certificates)
                    if (subject.Equals(certificate.Subject))
                    {
                        store.Remove(certificate);
                        success = true;
                    }

                store.Close();
            }

            return success;
        }

        #region IBankMasterCardService Methods

        public NewCardResults RequestNewCard(string password)
        {
            if (!Thread.CurrentPrincipal.IsInRole("Clients"))
            {
                Task.Run(() =>
                {
                    ProxyPool.GetProxy<IBankAuditService>().Log(new EventLogData(
                        _applicationName,
                        ExtractUsernameFromFullName(Thread.CurrentPrincipal.Identity.Name),
                        "Request for new card failed client isn't in client's role.",
                        EventLogEntryType.FailureAudit));
                });
                throw new SecurityException("Principal isn't part of Clients role.");
            }

            try
            {
                var clientName = ExtractUsernameFromFullName(Thread.CurrentPrincipal.Identity.Name);

                Console.WriteLine($"Client {clientName} requested new card.");

                Task.Run(() =>
                {
                    ProxyPool.GetProxy<IBankAuditService>().Log(new EventLogData(
                        _applicationName,
                        clientName,
                        "Request for new card!",
                        EventLogEntryType.Information));
                });

                if (CertificateManager.Instance.GetCertificateFromStore(
                        StoreLocation.LocalMachine,
                        StoreName.My,
                        clientName) != null)
                    throw new InvalidOperationException("You already have issued card.");

                RevokeCertificate(clientName);

                var CACertificate = CertificateManager.Instance.GetCACertificate();
                CertificateManager.Instance.CreateAndStoreNewCertificate(
                    clientName,
                    password,
                    CACertificate,
                    BankAppConfig.BankTransactionServiceCertificatePath);

                var cert = CertificateManager.Instance.GetCertificateFromStore(
                    StoreLocation.LocalMachine,
                    StoreName.My,
                    clientName);

                if (cert == null) throw new ArgumentNullException(nameof(cert));

                var resultData = new NewCardResults
                {
                    PinCode = GenerateRandomPin()
                };

                var client = BankCache.GetClientFromCache(_bankCache, clientName);
                client.ResetPin(null, resultData.PinCode);

                Task.Run(() =>
                {
                    ProxyPool.GetProxy<IBankAuditService>().Log(new EventLogData(
                        _applicationName,
                        clientName,
                        "Successfully created a card!",
                        EventLogEntryType.Information));
                });

                _bankCache.StoreData();

                var replicationData = new ReplicationItem(
                    client,
                    ReplicationType.UserData | ReplicationType.CertificateData,
                    cert);
                _replicatorProxy.ReplicateData(replicationData);


                return resultData;
            }
            catch (ArgumentNullException ane)
            {
                Task.Run(() =>
                {
                    ProxyPool.GetProxy<IBankAuditService>().Log(new EventLogData(
                        _applicationName,
                        Thread.CurrentPrincipal.Identity.Name,
                        ane.Message,
                        EventLogEntryType.Error));
                });

                throw new FaultException<CustomServiceException>(new CustomServiceException(ane.Message + "was null!"));
            }
        }

        public bool RevokeExistingCard(string pin)
        {
            if (!Thread.CurrentPrincipal.IsInRole("Clients"))
            {
                Task.Run(() =>
                {
                    ProxyPool.GetProxy<IBankAuditService>().Log(new EventLogData(
                        _applicationName,
                        ExtractUsernameFromFullName(Thread.CurrentPrincipal.Identity.Name),
                        "Request for new card failed client isn't in client's role.",
                        EventLogEntryType.FailureAudit));
                });
                throw new SecurityException("Client isn't in required role.");
            }

            try
            {
                // Check if client exists
                var clientName = ExtractUsernameFromFullName(Thread.CurrentPrincipal.Identity.Name);
                var client = default(IClient);
                client = BankCache.GetClientFromCache(_bankCache, clientName);

                if (client == null)
                    return false;

                // if he exists in the system, authorize him
                if (client.CheckPin(pin))
                {
                    Console.WriteLine($"Client {clientName} requested card revocation.");
                    Task.Run(() =>
                    {
                        ProxyPool.GetProxy<IBankAuditService>().Log(new EventLogData(
                            _applicationName,
                            clientName,
                            "Requested card revocation.",
                            EventLogEntryType.Information));
                    });

                    var cert = CertificateManager.Instance.GetCertificateFromStore(
                        StoreLocation.LocalMachine,
                        StoreName.My,
                        clientName);

                    if (cert == null) return false;

                    var revoked = RevokeCertificate(clientName);

                    if (revoked)
                    {
                        Task.Run(() =>
                        {
                            ProxyPool.GetProxy<IBankAuditService>().Log(new EventLogData(
                                _applicationName,
                                clientName,
                                "Successfully revoked the card.",
                                EventLogEntryType.Information));
                        });

                        client.Pin = null;
                        _bankCache.StoreData();

                        // We also replicate client data since pin wa removed
                        var replicationData = new ReplicationItem(
                            client,
                            ReplicationType.UserData | ReplicationType.CertificateData |
                            ReplicationType.RevokeCertificate,
                            cert);
                        _replicatorProxy.ReplicateData(replicationData);
                    }

                    return revoked;
                }

                Task.Run(() =>
                {
                    ProxyPool.GetProxy<IBankAuditService>().Log(new EventLogData(
                        _applicationName,
                        clientName,
                        "Invalid pin.",
                        EventLogEntryType.FailureAudit));
                });

                throw new SecurityException("Invalid pin.");
            }
            catch (ArgumentNullException ane)
            {
                Task.Run(() =>
                {
                    ProxyPool.GetProxy<IBankAuditService>().Log(new EventLogData(
                        _applicationName,
                        Thread.CurrentPrincipal.Identity.Name,
                        ane.Message,
                        EventLogEntryType.Error));
                });

                throw;
            }
        }

        public NewCardResults RequestResetPin()
        {
            if (!Thread.CurrentPrincipal.IsInRole("Clients"))
            {
                Task.Run(() =>
                {
                    ProxyPool.GetProxy<IBankAuditService>().Log(new EventLogData(
                        _applicationName,
                        ExtractUsernameFromFullName(Thread.CurrentPrincipal.Identity.Name),
                        "Request for new card failed client isn't in client's role.",
                        EventLogEntryType.FailureAudit));
                });
                throw new SecurityException("Client isn't in required role.");
            }

            var clientName = ExtractUsernameFromFullName(Thread.CurrentPrincipal.Identity.Name);
            try
            {
                var client = BankCache.GetClientFromCache(_bankCache, clientName);

                Console.WriteLine("Client requested pin reset.");

                Task.Run(() =>
                {
                    ProxyPool.GetProxy<IBankAuditService>().Log(new EventLogData(
                        _applicationName,
                        clientName,
                        "Requested pin reset.",
                        EventLogEntryType.Information));
                });

                var results = new NewCardResults {PinCode = GenerateRandomPin()};

                client.ResetPin(null, results.PinCode);
                Task.Run(() =>
                {
                    ProxyPool.GetProxy<IBankAuditService>().Log(new EventLogData(
                        _applicationName,
                        clientName,
                        "New pin generated.",
                        EventLogEntryType.Information));
                });

                _replicatorProxy.ReplicateData(new ReplicationItem(client));

                return results;
            }
            catch (ArgumentNullException ane)
            {
                Task.Run(() =>
                {
                    ProxyPool.GetProxy<IBankAuditService>().Log(new EventLogData(
                        _applicationName,
                        clientName,
                        ane.Message,
                        EventLogEntryType.Error));
                });

                throw new ArgumentNullException(ane.Message);
            }
        }

        public bool ExtendCard(string password)
        {
            var clientName = ExtractUsernameFromFullName(Thread.CurrentPrincipal.Identity.Name);

            if (!Thread.CurrentPrincipal.IsInRole("Clients"))
            {
                Task.Run(() =>
                {
                    ProxyPool.GetProxy<IBankAuditService>().Log(new EventLogData(
                        _applicationName,
                        clientName,
                        $"({nameof(BankServiceApp)}) [BankMasterCardService] Client isn't part of Clients group.",
                        EventLogEntryType.FailureAudit));
                });
                throw new SecurityException("Principal isn't part of Clients role.");
            }

            try
            {
                Console.WriteLine($"Client {clientName} requested extension.");

                Task.Run(() =>
                {
                    ProxyPool.GetProxy<IBankAuditService>().Log(new EventLogData(
                        _applicationName,
                        clientName,
                        "Requested extension.",
                        EventLogEntryType.Information));
                });

                RevokeCertificate(clientName);

                var CACertificate = CertificateManager.Instance.GetCACertificate();
                CertificateManager.Instance.CreateAndStoreNewCertificate(
                    clientName,
                    password,
                    CACertificate,
                    BankAppConfig.BankTransactionServiceCertificatePath);

                var newCert = CertificateManager.Instance.GetCertificateFromStore(
                    StoreLocation.LocalMachine,
                    StoreName.My,
                    clientName);

                if (newCert == null)
                    return false;

                Task.Run(() =>
                {
                    ProxyPool.GetProxy<IBankAuditService>().Log(new EventLogData(
                        _applicationName,
                        clientName,
                        "Successfully extended the card.",
                        EventLogEntryType.Information));
                });

                var replicationItem = new ReplicationItem(null, ReplicationType.CertificateData, newCert);
                _replicatorProxy.ReplicateData(replicationItem);

                return true;
            }
            catch (ArgumentNullException ane)
            {
                Task.Run(() =>
                {
                    ProxyPool.GetProxy<IBankAuditService>().Log(new EventLogData(
                        _applicationName,
                        Thread.CurrentPrincipal.Identity.Name,
                        ane.Message,
                        EventLogEntryType.Error));
                });

                throw new FaultException<CustomServiceException>(new CustomServiceException(ane.Message + "was null!"),
                    $"{ane.Message} was null!");
            }
        }

        public void Login()
        {
            var principal = Thread.CurrentPrincipal;
            if (!principal.IsInRole("Clients"))
            {
                Task.Run(() =>
                {
                    ProxyPool.GetProxy<IBankAuditService>().Log(new EventLogData(
                        _applicationName,
                        Thread.CurrentPrincipal.Identity.Name,
                        "User isn't in Clients role.",
                        EventLogEntryType.FailureAudit));
                });

                throw new SecurityException("User isn't in Clients role.");
            }

            Task.Run(() =>
            {
                ProxyPool.GetProxy<IBankAuditService>().Log(new EventLogData(
                    _applicationName,
                    Thread.CurrentPrincipal.Identity.Name,
                    "Successfully logged in.",
                    EventLogEntryType.Information));
            });
        }

        public ServiceState CheckState()
        {
            var principal = Thread.CurrentPrincipal;
            if (!principal.IsInRole("Clients"))
            {
                Task.Run(() =>
                {
                    ProxyPool.GetProxy<IBankAuditService>().Log(new EventLogData(
                        _applicationName,
                        Thread.CurrentPrincipal.Identity.Name,
                        "User isn't in Clients role.",
                        EventLogEntryType.FailureAudit));
                });

                throw new SecurityException("User isn't in Clients role.");
            }

            return _arbitrationServiceProvider.State;
        }

        #endregion
    }
}