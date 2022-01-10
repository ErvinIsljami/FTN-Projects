using Common.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Common.DataContracts.Dtos;
using System.Diagnostics;

namespace Common.Communication
{
	/// <summary>
	/// Represents client proxy for given interface with windows authentication management.
	/// </summary>
	/// <typeparam name="T">Interface which will be used for proxy.</typeparam>
	public class AuditClientProxy : ChannelFactory<IAuditService>, IAudit, IDisposable
	{
		private readonly string bankService = "bankservice";
		private IAuditService _proxy;

		/// <summary>
		/// Initializes a new instance of <see cref="WindowsClientProxy<T> class."/>
		/// </summary>
		/// <param name="serviceAddress">Service address.</param>
		/// <param name="serviceEndpointName">Service endpoint name.</param>
		public AuditClientProxy(string serviceAddress, string serviceEndpointName) : 
			base(new NetTcpBinding(), SetUpEndpoint(serviceAddress, serviceEndpointName))
		{
			_proxy = CreateChannel();
		}


		/// <inheritdoc/>
		public void Dispose()
		{
			_proxy = null;
		}

		private static EndpointAddress SetUpEndpoint(string serviceAddress, string serviceEndpointName)
		{
			return new EndpointAddress($"{serviceAddress}/{serviceEndpointName}");
		}

		public void Log(string accountName, string logMessage, EventLogEntryType eventLogEntryType)
		{
			try
			{
				_proxy.Log(new EventLogData(bankService, accountName, logMessage, eventLogEntryType));
			}
			catch { }
		}

		private static NetTcpBinding CreateNewBinding()
		{
			var binding = new NetTcpBinding();
			binding.CloseTimeout = binding.OpenTimeout = binding.ReceiveTimeout = binding.SendTimeout = new TimeSpan(1, 0, 0, 0);

			return binding;
		}
	}
}
