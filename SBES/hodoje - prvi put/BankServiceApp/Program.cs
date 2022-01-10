using System;
using System.Security.Principal;
using BankServiceApp.AccountStorage;
using BankServiceApp.Arbitration;
using BankServiceApp.Replication;
using Common;
using Common.CertificateManager;
using Common.ServiceContracts;

namespace BankServiceApp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var principal = new WindowsPrincipal(WindowsIdentity.GetCurrent());
            if (!principal.IsInRole("BankServices"))
            {
                Console.WriteLine($"Only user in BankServices role can run {nameof(BankServiceApp)}");
                Console.WriteLine("Press any key to exit...");
                Console.ReadLine();
                return;
            }

            using (ICache bankCache = new BankCache())
            {
                ServiceLocator.RegisterService(bankCache);
                using (IArbitrationServiceProvider arbitrationService = new ArbitrationServiceProvider())
                {
                    ServiceLocator.RegisterService(arbitrationService);
                    using (var replicatorProxy = new ReplicatorProxy())
                    {
                        ProxyPool.RegisterProxy<IReplicator>(replicatorProxy);

                        using (var auditProxy = new BankAuditServiceProxy())
                        {
                            ProxyPool.RegisterProxy<IBankAuditService>(auditProxy);

                            if (CertificateManager.Instance.GetCACertificate() == null)
                                try
                                {
                                    var caCertificate = CertificateManager.Instance.GetPrivateCertificateFromFile(
                                        BankAppConfig.CACertificatePath,
                                        BankAppConfig.CACertificatePass);
                                    CertificateManager.Instance.SetCACertificate(caCertificate);
                                }
                                catch
                                {
                                    Console.ReadLine();
                                    throw;
                                }

                            arbitrationService.RegisterService(new BankServicesHost());
                            arbitrationService.OpenServices();
                            Console.ReadLine();
                        }
                    }
                }
            }
        }
    }
}