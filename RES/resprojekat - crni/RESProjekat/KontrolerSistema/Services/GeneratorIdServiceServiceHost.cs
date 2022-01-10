using Common.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace SystemController.Services
{
	public class GeneratorIdServiceServiceHost : IDisposable
	{
		private readonly string _generatorIdServiceAddress = "net.tcp://localhost:7777";
		private readonly string _generatorIdServiceEndpointName = "GeneratorIdService";
		private readonly ServiceHost _generatorIdServiceServiceHost;
		private readonly NetTcpBinding _binding;

		public GeneratorIdServiceServiceHost()
		{
			_binding = new NetTcpBinding();
			_generatorIdServiceServiceHost = new ServiceHost(typeof(GeneratorIdService));
			_generatorIdServiceServiceHost.AddServiceEndpoint(typeof(IGeneratorIdService), _binding, $"{_generatorIdServiceAddress}/{_generatorIdServiceEndpointName}");
		}

		public void OpenService()
		{
			try
			{
				_generatorIdServiceServiceHost.Open();
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Generator Id Service failed to open with an error: {ex.Message}");
			}
		}

		public void CloseService()
		{
			try
			{
				_generatorIdServiceServiceHost.Close();
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Generator Id Service failed to close with an error: {ex.Message}");
			}
		}

		public void Dispose()
		{
			(_generatorIdServiceServiceHost as IDisposable).Dispose();
		}
	}
}
