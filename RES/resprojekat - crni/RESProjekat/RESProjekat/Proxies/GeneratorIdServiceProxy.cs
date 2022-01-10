using Common.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace LocalController.Proxies
{
	public class GeneratorIdServiceProxy : IDisposable
	{
		private IGeneratorIdService _generatorIdServiceProxy;
		private ChannelFactory<IGeneratorIdService> _channelFactory;

		public GeneratorIdServiceProxy()
		{
			var binding = new NetTcpBinding();
			var address = new EndpointAddress("net.tcp://localhost:7777/GeneratorIdService");
			_channelFactory = new ChannelFactory<IGeneratorIdService>(binding, address);
		}

		public string GetNewGeneratorId()
		{
			string newGeneratorId = "";
			try
			{
				if (_generatorIdServiceProxy == null)
				{
					_generatorIdServiceProxy = _channelFactory.CreateChannel();
				}
				newGeneratorId = _generatorIdServiceProxy.GenerateGeneratorId();
			}
			catch (Exception e)
			{
				Console.WriteLine($"An error has occurred while logging in: {e.Message}");
			}
			return newGeneratorId;
		}

		public void Dispose()
		{
			_generatorIdServiceProxy = null;
			_channelFactory = null;
		}
	}
}
