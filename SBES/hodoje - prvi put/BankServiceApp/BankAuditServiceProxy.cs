using System;
using System.Net.Security;
using System.ServiceModel;
using Common.EventLogData;
using Common.ServiceContracts;

namespace BankServiceApp
{
    public class BankAuditServiceProxy : IBankAuditService, IDisposable
    {
        private IBankAuditService _auditProxy;
        private ChannelFactory<IBankAuditService> _channelFactory;

        public BankAuditServiceProxy()
        {
            var binding = new NetTcpBinding(SecurityMode.Transport);
            binding.Security.Transport.ProtectionLevel = ProtectionLevel.EncryptAndSign;
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
            binding.OpenTimeout = binding.CloseTimeout = TimeSpan.FromSeconds(2);

            var address = new EndpointAddress(BankAppConfig.BankAuditServiceEndpoint);

            _channelFactory = new ChannelFactory<IBankAuditService>(binding, address);
        }

        public void Log(EventLogData eventLogData)
        {
            try
            {
                if (_auditProxy == null) _auditProxy = _channelFactory.CreateChannel();
                _auditProxy.Log(eventLogData);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error logging data on audit service: {e.Message}");
            }
        }

        public void Dispose()
        {
            _channelFactory = null;
            _auditProxy = null;
        }
    }
}