using Common.CertificateManagement;
using System;
using System.Collections.Generic;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.ServiceModel;
using System.Threading;

namespace Common.Communication
{
	/// <summary>
	/// Represents client proxy for given interface with certificate management.
	/// </summary>
	/// <typeparam name="T">Interface which will be used for proxy.</typeparam>
	public class CertificateClientProxy<T> : DuplexChannelFactory<T>, IDisposable where T : class
	{
		private T _proxy;

		/// <summary>
		/// Initializes a new instance of <see cref="CertificateClientProxy<T> class."/>
		/// </summary>
		/// <param name="callback">Service callback.</param>
		/// <param name="serviceAddress">Service address.</param>
		/// <param name="serviceEndpointName">Service endpoint name.</param>
		public CertificateClientProxy(object callback, string serviceAddress, string serviceEndpointName, X509Certificate2 certificate)
			: base(callback, SetUpBinding(), SetUpEndpoint(serviceAddress, serviceEndpointName))
		{
			Credentials.ServiceCertificate.Authentication.CertificateValidationMode = System.ServiceModel.Security.X509CertificateValidationMode.ChainTrust;
			Credentials.ServiceCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;
			Credentials.ClientCertificate.Certificate = certificate;
			_proxy = this.CreateChannel();
		}

		/// <summary>
		/// Proxy used for communication with service.
		/// </summary>
		public T Proxy
		{
			get
			{
				_proxy = this.CreateChannel();

				return _proxy;
			}
		}

		/// <inheritdoc/>
		public void Dispose()
		{
			_proxy = null;
		}

		private static EndpointAddress SetUpEndpoint(string serviceAddress, string serviceEndpointName)
		{
			return
				new EndpointAddress(new Uri($"{serviceAddress}/{serviceEndpointName}"), new DnsEndpointIdentity("bankservice"));
		}

		private static NetTcpBinding SetUpBinding()
		{
			var binding = new NetTcpBinding(SecurityMode.Transport);
			binding.CloseTimeout = binding.OpenTimeout = binding.ReceiveTimeout = binding.SendTimeout = new TimeSpan(1, 0, 0, 0);
			binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;
			binding.Security.Transport.ProtectionLevel = ProtectionLevel.EncryptAndSign;

			return binding;
		}
	}
}
