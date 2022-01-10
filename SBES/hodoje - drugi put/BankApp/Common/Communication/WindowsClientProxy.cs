using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Common.Communication
{
	/// <summary>
	/// Represents client proxy for given interface with windows authentication management.
	/// </summary>
	/// <typeparam name="T">Interface which will be used for proxy.</typeparam>
	public class WindowsClientProxy<T> : IDisposable where T : class
	{
		private T _proxy;
		private ChannelFactory<T> _channelFactory;

		/// <summary>
		/// Initializes a new instance of <see cref="WindowsClientProxy<T> class."/>
		/// </summary>
		/// <param name="serviceAddress">Service address.</param>
		/// <param name="serviceEndpointName">Service endpoint name.</param>
		public WindowsClientProxy(string serviceAddress, string serviceEndpointName)
		{
			var binding = SetUpBinding();
			var endpointAddress = SetUpEndpoint(serviceAddress, serviceEndpointName);
			_channelFactory = new ChannelFactory<T>(binding, endpointAddress);
			_proxy = _channelFactory.CreateChannel();
		}

		/// <summary>
		/// Proxy used for communication with service.
		/// </summary>
		public T Proxy
		{
			get
			{
				_proxy = _channelFactory.CreateChannel();
				return _proxy;
			}			
		}

		/// <inheritdoc/>
		public void Dispose()
		{
			_proxy = null;
			_channelFactory = null;
		}

		private EndpointAddress SetUpEndpoint(string serviceAddress, string serviceEndpointName)
		{
			return new EndpointAddress($"{serviceAddress}/{serviceEndpointName}");
		}

		private NetTcpBinding SetUpBinding()
		{
			var binding = new NetTcpBinding(SecurityMode.Transport);
			binding.CloseTimeout = binding.OpenTimeout = binding.ReceiveTimeout = binding.SendTimeout = new TimeSpan(1, 0, 0, 0);
			binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;			
			binding.Security.Transport.ProtectionLevel = ProtectionLevel.Sign;

			return binding;
		}
	}
}
