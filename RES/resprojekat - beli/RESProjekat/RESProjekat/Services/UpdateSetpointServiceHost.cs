using Common.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace LocalController.Services
{
	public class UpdateSetpointServiceHost
	{
		private readonly string _updateSetpointServiceAddress = "net.tcp://localhost";
		private readonly ServiceHost _updateSetpointServiceServiceHost;
		private readonly NetTcpBinding _binding;

		public UpdateSetpointServiceHost(double port, string lcID)
		{
			_binding = new NetTcpBinding();
			_updateSetpointServiceServiceHost = new ServiceHost(typeof(UpdateSetpointService));
			_updateSetpointServiceServiceHost.AddServiceEndpoint(typeof(IUpdateSetpoint), _binding, $"{_updateSetpointServiceAddress}:{port}/{lcID}");
		}

		public void OpenService()
		{
			try
			{
				_updateSetpointServiceServiceHost.Open();
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Update Setpoint Service failed to open with an error: {ex.Message}");
			}
		}

		public void CloseService()
		{
			try
			{
				_updateSetpointServiceServiceHost.Close();
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Update Setpoint Service failed to close with an error: {ex.Message}");
			}
		}

		public void Dispose()
		{
			(_updateSetpointServiceServiceHost as IDisposable).Dispose();
		}
	}
}
