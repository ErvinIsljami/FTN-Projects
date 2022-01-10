using Common.Contracts;
using Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace LocalController.Proxies
{
	public class LcToScServiceProxy : IDisposable
	{
		public static LoginRequestResponse LRR { get; set; }
		private ILcToScService _lcToScServiceProxy;
		private ChannelFactory<ILcToScService> _channelFactory;

		public LcToScServiceProxy()
		{
			var binding = new NetTcpBinding();
			var address = new EndpointAddress("net.tcp://localhost:4444/LcToScService");
			_channelFactory = new ChannelFactory<ILcToScService>(binding, address);
		}

		public void Login(List<Generator> lcGenerators)
		{
			try
			{
				if(_lcToScServiceProxy == null)
				{
					_lcToScServiceProxy = _channelFactory.CreateChannel();
				}
				LRR = _lcToScServiceProxy.Login(lcGenerators);
			}
			catch(Exception e)
			{
				Console.WriteLine($"An error has occurred while logging in: {e.Message}");
			}
		}

		public void SendMeasurements(string lkGuid, List<Generator> lkGenerators)
		{
			try
			{
				if(_lcToScServiceProxy == null)
				{
					_lcToScServiceProxy = _channelFactory.CreateChannel();
				}
				_lcToScServiceProxy.SendMeasurements(LRR.LcId, lkGenerators);
			}
			catch(Exception e)
			{
				Console.WriteLine($"An error has occurred while sending measurements: {e.Message}");
			}
		}

		public void Dispose()
		{
			_lcToScServiceProxy = null;
			_channelFactory = null;
		}
	}
}
