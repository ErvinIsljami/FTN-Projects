using System;
using System.Net.Security;
using System.Security;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Security;
using Common;
using Common.CertificateManager;
using Common.DataEncapsulation;
using Common.ServiceContracts;
using Common.Transaction;

namespace ClientApp
{
    public class ClientProxy : IBankMasterCardService, IBankTransactionService
    {
        private readonly IBankMasterCardService _cardServiceProxy;
        private EndpointAddress _cardServiceEndpointAddress;

        private X509Certificate2 _serverCertificate;
        private EndpointAddress _transactionServiceEndpointAddress;

        private IBankTransactionService _transactionServiceProxy;
        private ChannelFactory<IBankTransactionService> _transactionServiceProxyFactory;

        public ClientProxy(string username, SecureString password)
        {
            var i = 0;
            while (true)
            {
                SetUpEndpoints(i++ % 2);
                var cardServiceFactory =
                    new ChannelFactory<IBankMasterCardService>(SetUpWindowsAuthBinding(), _cardServiceEndpointAddress);
                cardServiceFactory.Credentials.Windows.ClientCredential.UserName = username;
                cardServiceFactory.Credentials.Windows.ClientCredential.SecurePassword = password;
                _cardServiceProxy = cardServiceFactory.CreateChannel();
                if (_cardServiceProxy.CheckState() != ServiceState.Hot)
                {
                    ((IClientChannel) _cardServiceProxy).Close();
                    cardServiceFactory.Close();
                }
                else
                {
                    break;
                }
            }

            _cardServiceProxy.Login();
        }

        public NewCardResults RequestNewCard(string password)
        {
            var newCardResults = new NewCardResults();
            try
            {
                newCardResults = _cardServiceProxy.RequestNewCard(password);
            }
            catch (FaultException ex)
            {
                Console.WriteLine("Error: {0}", ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: {0}", ex.Message);
                Console.ReadLine();
            }

            return newCardResults;
        }

        public bool RevokeExistingCard(string pin)
        {
            var Result = false;
            try
            {
                Result = _cardServiceProxy.RevokeExistingCard(pin);
            }
            catch (FaultException ex)
            {
                Console.WriteLine("Error: {0}", ex.Message);
            }

            return Result;
        }

        public NewCardResults RequestResetPin()
        {
            var newCardResults = new NewCardResults();
            try
            {
                newCardResults = _cardServiceProxy.RequestResetPin();
            }
            catch (FaultException ex)
            {
                Console.WriteLine("Error: {0}", ex.Message);
            }

            return newCardResults;
        }

        public void Login()
        {
            try
            {
                _cardServiceProxy.Login();
            }
            catch (SecurityAccessDeniedException ex)
            {
                Console.WriteLine($"User failed to login reason: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw;
            }
        }

        public ServiceState CheckState()
        {
            try
            {
                return _cardServiceProxy.CheckState();
            }
            catch (FaultException ex)
            {
                Console.WriteLine("Error: {0}", ex.Message);
            }

            return ServiceState.Standby;
        }

        public bool ExtendCard(string password)
        {
            var Result = false;
            try
            {
                Result = _cardServiceProxy.ExtendCard(password);
            }
            catch (FaultException ex)
            {
                Console.WriteLine("Error: {0}", ex.Message);
            }

            return Result;
        }

        public bool ExecuteTransaction(byte[] signature, ITransaction transaction)
        {
            var Result = false;

            try
            {
                Result = _transactionServiceProxy.ExecuteTransaction(signature, transaction);
            }
            catch (FaultException ex)
            {
                Console.WriteLine("Error: {0}", ex.Message);
                throw;
            }

            return Result;
        }

        public decimal CheckBalance(byte[] signature, ITransaction transaction)
        {
            decimal Result = 0;
            try
            {
                Result = _transactionServiceProxy.CheckBalance(signature, transaction);
            }
            catch (FaultException ex)
            {
                Console.WriteLine("Error: {0}", ex.Message);
            }

            return Result;
        }

        private void SetUpEndpoints(int i)
        {
            _cardServiceEndpointAddress = new EndpointAddress(
                new Uri(ClientAppConfig.MasterCardServiceAddress[i]));

            _serverCertificate = CertificateManager.Instance.GetCertificateFromStore(
                StoreLocation.LocalMachine,
                StoreName.Root,
                ClientAppConfig.ServiceCertificateCN);

            _transactionServiceEndpointAddress = new EndpointAddress(
                new Uri(ClientAppConfig.TransactionServiceAddress[i]),
                new X509CertificateEndpointIdentity(_serverCertificate));
        }

        public void OpenTransactionServiceProxy(X509Certificate2 clientCertificate)
        {
            _transactionServiceProxyFactory =
                new ChannelFactory<IBankTransactionService>(SetupCertAuthBinding(), _transactionServiceEndpointAddress);
            _transactionServiceProxyFactory.Credentials.ClientCertificate.Certificate = clientCertificate;
            _transactionServiceProxy = _transactionServiceProxyFactory.CreateChannel();
        }

        private NetTcpBinding SetUpWindowsAuthBinding()
        {
            var binding = new NetTcpBinding(SecurityMode.Transport);
            binding.Security.Transport.ProtectionLevel =
                ProtectionLevel.EncryptAndSign;
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
            binding.OpenTimeout = binding.CloseTimeout = TimeSpan.FromSeconds(2);

            return binding;
        }

        private NetTcpBinding SetupCertAuthBinding()
        {
            var binding = new NetTcpBinding(SecurityMode.Transport);
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;
            binding.Security.Transport.ProtectionLevel = ProtectionLevel.EncryptAndSign;
            binding.OpenTimeout = binding.CloseTimeout = TimeSpan.FromSeconds(2);

            return binding;
        }
    }
}