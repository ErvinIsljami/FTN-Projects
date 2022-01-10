using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
	/// <summary>
	/// Loads app settings from App.config
	/// </summary>
	public static class ClientConfig
	{
		public const string BankServiceAddressConfigName = "BankServiceAddress";
		public const string UserServiceEndpointNameConfigName = "UserServiceEndpointName";
		public const string AdminServiceEndpointNameConfigName = "AdminServiceEndpointName";

		static ClientConfig()
		{
			BankServiceAddress = ConfigurationManager.AppSettings[BankServiceAddressConfigName];
			UserServiceEndpoint = ConfigurationManager.AppSettings[UserServiceEndpointNameConfigName];
			AdminServiceEndpoint = ConfigurationManager.AppSettings[AdminServiceEndpointNameConfigName];
		}

		public static string BankServiceAddress { get; }
		public static string UserServiceEndpoint { get; }
		public static string AdminServiceEndpoint { get; }
	}
}
