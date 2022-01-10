using Common.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace SystemController.Services
{
	public class LcToScServiceServiceHost : IDisposable
	{
		private readonly string _lcToScServiceAddress = "net.tcp://localhost:4444";
		private readonly string _lcToScServiceEndpointName = "LcToScService";
		private readonly ServiceHost _lcToScServiceServiceHost;
		private readonly NetTcpBinding _binding;

		public LcToScServiceServiceHost(LcToScService serviceInstance)
		{
			_binding = new NetTcpBinding();
			_lcToScServiceServiceHost = new ServiceHost(serviceInstance);
			_lcToScServiceServiceHost.AddServiceEndpoint(typeof(ILcToScService), _binding, $"{_lcToScServiceAddress}/{_lcToScServiceEndpointName}");
		}

		public void OpenService()
		{
			try
			{
				_lcToScServiceServiceHost.Open();
			}
			catch (Exception ex)
			{
				Console.WriteLine($"LC to SC Service failed to open with an error: {ex.Message}");
			}
		}

		public void CloseService()
		{
			try
			{
				_lcToScServiceServiceHost.Close();
			}
			catch (Exception ex)
			{
				Console.WriteLine($"LC to SC Service failed to close with an error: {ex.Message}");
			}
		}

		public void Dispose()
		{
			(_lcToScServiceServiceHost as IDisposable).Dispose();
		}
	}
}
