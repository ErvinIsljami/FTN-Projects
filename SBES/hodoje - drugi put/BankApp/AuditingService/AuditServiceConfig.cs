using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditingService
{
	/// <summary>
	/// Loads app settings from App.config
	/// </summary>
	internal static class AuditServiceConfig
	{
		public const string LogNameConfigName = "LogName";
		public const string AuditServiceAddressConfigName = "AuditServiceAddress";
		public const string AuditServiceEndpointNameConfigName = "AuditServiceEndpointName";

		static AuditServiceConfig()
		{
			LogName = ConfigurationManager.AppSettings[LogNameConfigName];
			AuditServiceAddress = ConfigurationManager.AppSettings[AuditServiceAddressConfigName];
			AuditServiceEndpointName = ConfigurationManager.AppSettings[AuditServiceEndpointNameConfigName];
		}

		public static string LogName { get; }
		public static string AuditServiceAddress { get; }
		public static string AuditServiceEndpointName { get; }
	}
}
