using System;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Security;
using BankServiceApp.BankServices;
using BankServiceApp.ServiceHosts;
using Common.CertificateManager;
using Common.ServiceContracts;

namespace BankServiceApp
{
    public class BankServicesHost : IServiceHost, IDisposable
    {
        private readonly ServiceHost _cardHost;
        private readonly ServiceHost _transactionServiceHost;

        public BankServicesHost()
        {
            #region MasterCardServiceSetup

            var masterCardHostBinding = SetupWindowsAuthBinding();
            var masterCardServiceEndpoint = $"{BankAppConfig.MyEndpoint}/{BankAppConfig.MasterCardServiceName}";

            _cardHost = new ServiceHost(typeof(BankMasterCardService));
            _cardHost.AddServiceEndpoint(typeof(IBankMasterCardService), masterCardHostBinding,
                masterCardServiceEndpoint);

            #endregion

            #region TransactionServiceSetup

            var transactionServiceCertificate = LoadServiceCertificate(
                BankAppConfig.BankTransactionServiceCertificatePath,
                BankAppConfig.BankTransactionServiceSubjectName,
                BankAppConfig.BankTransactionServiceCertificatePassword);

            var transactionHostBinding = SetupCertificateAuthBinding();
            var transactionServiceEndpoint = $"{BankAppConfig.MyEndpoint}/{BankAppConfig.TransactionServiceName}";
            _transactionServiceHost = new ServiceHost(typeof(BankTransactionService));
            _transactionServiceHost.AddServiceEndpoint(typeof(IBankTransactionService), transactionHostBinding,
                transactionServiceEndpoint);

            _transactionServiceHost.Credentials.ServiceCertificate.Certificate = transactionServiceCertificate;
            _transactionServiceHost.Credentials.ClientCertificate.Authentication.CertificateValidationMode =
                X509CertificateValidationMode.ChainTrust;
            _transactionServiceHost.Credentials.ClientCertificate.Authentication.RevocationMode =
                X509RevocationMode.NoCheck;

            #endregion
        }

        #region IDisposable Methods

        public void Dispose()
        {
            (_cardHost as IDisposable).Dispose();
            (_transactionServiceHost as IDisposable).Dispose();
        }

        #endregion

        private NetTcpBinding SetupWindowsAuthBinding()
        {
            var binding = new NetTcpBinding(SecurityMode.Transport);
            binding.Security.Transport.ProtectionLevel =
                ProtectionLevel.EncryptAndSign;

            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
            binding.OpenTimeout = binding.CloseTimeout = TimeSpan.FromSeconds(2);

            return binding;
        }

        private NetTcpBinding SetupCertificateAuthBinding()
        {
            var binding = new NetTcpBinding(SecurityMode.Transport);
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;
            binding.Security.Transport.ProtectionLevel = ProtectionLevel.EncryptAndSign;
            binding.OpenTimeout = binding.CloseTimeout = TimeSpan.FromSeconds(2);

            return binding;
        }

        private X509Certificate2 LoadServiceCertificate(string path, string name, string password)
        {
            var certificate = default(X509Certificate2);
            try
            {
                certificate = CertificateManager.Instance.GetPrivateCertificateFromFile($"{path}{name}.pfx", password);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading service certificate from file. Reason: {ex.Message}");
            }

            return certificate;
        }

        private void CardServiceOpen()
        {
            try
            {
                _cardHost.Open();
            }
            catch (Exception ex)
            {
                Console.WriteLine("CardService failed to open with an error: {0}", ex.Message);
            }
        }

        private void CardServiceClose()
        {
            try
            {
                _cardHost.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("CardService failed to close with an error: {0}", ex.Message);
                throw;
            }
        }

        private void TransactionServiceOpen()
        {
            try
            {
                _transactionServiceHost.Open();
            }
            catch (Exception ex)
            {
                Console.WriteLine("TransactionService failed to open with an error: {0}", ex.Message);
                throw;
            }
        }

        private void TransactionServiceClose()
        {
            try
            {
                _transactionServiceHost.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("TransationService failed to close with an error: {0}", ex.Message);
                throw;
            }
        }

        #region IServiceHost Methods

        public void OpenService()
        {
            CardServiceOpen();
            Console.WriteLine("CardServiceHost is opened..");
            TransactionServiceOpen();
            Console.WriteLine("TransationServiceHost is opened..");
        }

        public void CloseService()
        {
            TransactionServiceClose();
            CardServiceClose();
        }

        #endregion
    }
}