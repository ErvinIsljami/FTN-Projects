using Common.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace AuditingService
{
	public class AuditServiceServiceHost : IDisposable
	{
		private readonly string _auditServiceAddress;
		private readonly string _auditServiceEndpointName;
		private readonly ServiceHost _auditServiceHost;
		private readonly NetTcpBinding _binding;

		public AuditServiceServiceHost()
		{
			_auditServiceAddress = AuditServiceConfig.AuditServiceAddress;
			_auditServiceEndpointName = AuditServiceConfig.AuditServiceEndpointName;
			_binding = SetUpBinding();
			_auditServiceHost = new ServiceHost(typeof(AuditService));
			_auditServiceHost.AddServiceEndpoint(typeof(IAuditService), _binding,
				$"{_auditServiceAddress}/{_auditServiceEndpointName}");
		}

		public void OpenService()
		{
			try
			{
				_auditServiceHost.Open();
			}
			catch (Exception ex)
			{
				Console.WriteLine($"AuditServiceHost failed to open with an error: {ex.Message}");
			}
		}

		public void CloseService()
		{
			try
			{
				_auditServiceHost.Close();
			}
			catch (Exception ex)
			{
				Console.WriteLine($"AuditServiceHost failed to close with an error: {ex.Message}");
			}
		}

		private NetTcpBinding SetUpBinding()
		{
			var binding = new NetTcpBinding();
			binding.Security.Mode = SecurityMode.Transport;
			binding.Security.Transport.ProtectionLevel = ProtectionLevel.EncryptAndSign;
			binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
			return binding;
		}

		public void Dispose()
		{
			(_auditServiceHost as IDisposable).Dispose();
		}
	}
}
