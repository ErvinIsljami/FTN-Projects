using Common.Contracts;
using Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace SystemController.Proxies
{
	public class UpdateSetpointServiceProxy
	{
		private IUpdateSetpoint _lcToScServiceProxy;
		private ChannelFactory<IUpdateSetpoint> _channelFactory;

		public UpdateSetpointServiceProxy(string lc, double port)
		{
			var binding = new NetTcpBinding();
            binding.ReceiveTimeout = new TimeSpan(0, 5, 0);
            binding.SendTimeout = new TimeSpan(0, 5, 0);
			var address = new EndpointAddress($"net.tcp://localhost:{port}/{lc}");
			_channelFactory = new ChannelFactory<IUpdateSetpoint>(binding, address);
            _lcToScServiceProxy = _channelFactory.CreateChannel();
        }

		public void SetPointUpdate(string lcId, List<SetpointArray> setpointArrays)
		{
            _lcToScServiceProxy.SetpointUpdate(setpointArrays);
		}

		public void Dispose()
		{
			_lcToScServiceProxy = null;
			_channelFactory = null;
		}
	}
}
