using Common.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SectorService
{
	/// <summary>
	/// Loads app settings from App.config
	/// </summary>
	public static class SectorConfig
	{
		public const string SectorQueueSizeConfigName = "SectorQueueSize";
		public const string SectorQueueTimeoutInSecondsConfigName = "SectorQueueTimeoutInSeconds";
		public const string AuditServiceAddressConfigName = "AuditServiceAddress";
		public const string AuditServiceEndpointNameConfigName = "AuditServiceEndpointName";
		public const string StartupConfirmationServiceAddressConfigName = "StartupConfirmationServiceAddress";
		public const string StartupConfirmationServiceEndpointNameConfigName = "StartupConfirmationServiceEndpointName";
		public const string AllSectorNamesConfigName = "AllSectorNames";
		public const string SectorsConfigName = "Sectors";
		public const string BankAliveServiceAddressConfigName = "BankAliveServiceAddress";
		public const string BankAliveServiceEndpointConfigName = "BankAliveServiceEndpointName";
		//public const string EncryptionKey = "EncryptionKey";

		static SectorConfig()
		{
			//EncryptionKeyValue = ConfigurationManager.AppSettings[EncryptionKey];
			BankAliveServiceAddress = ConfigurationManager.AppSettings[BankAliveServiceAddressConfigName];
			BankAliveServiceEndpointName = ConfigurationManager.AppSettings[BankAliveServiceEndpointConfigName];
			try
			{
				SectorQueueSize = Int32.Parse(ConfigurationManager.AppSettings[SectorQueueSizeConfigName]);
				SectorQueueTimeoutInSeconds = Int32.Parse(ConfigurationManager.AppSettings[SectorQueueTimeoutInSecondsConfigName]);
			}
			catch (Exception e)
			{
				throw new Exception("Invalid configuration. Expected a number.", e);
			}
			AuditServiceAddress = ConfigurationManager.AppSettings[AuditServiceAddressConfigName];
			AuditServiceEndpointName = ConfigurationManager.AppSettings[AuditServiceEndpointNameConfigName];
			StartupConfirmationServiceAddress = ConfigurationManager.AppSettings[StartupConfirmationServiceAddressConfigName];
			StartupConfirmationServiceEndpointName = ConfigurationManager.AppSettings[StartupConfirmationServiceEndpointNameConfigName];
			AllSectorNames = ConfigurationManager.AppSettings[AllSectorNamesConfigName].Split(',');

			string sectorsConfigJson = ConfigurationManager.AppSettings[SectorsConfigName];
			SectorsConfigs = GetSectorsConfig(sectorsConfigJson);
		}

		private static Dictionary<string, SectorAdditionalConfig> GetSectorsConfig(string sectorsConfigJson)
		{
			Dictionary<string, SectorAdditionalConfig> result = new Dictionary<string, SectorAdditionalConfig>();
			JObject sectorsConfigJObject = JsonConvert.DeserializeObject<JObject>(sectorsConfigJson);
			var sectors = sectorsConfigJObject["sectors"];
			
			foreach (var child in sectors.Children())
			{
				result.Add((child as JProperty).Name, child.First.ToObject<SectorAdditionalConfig>());
			}

			return result;
		}

		public static int SectorQueueSize { get; }
		public static int SectorQueueTimeoutInSeconds { get; }
		public static string AuditServiceAddress { get; }
		public static string AuditServiceEndpointName { get; }
		public static string StartupConfirmationServiceAddress { get; }
		public static string StartupConfirmationServiceEndpointName { get; }
		public static string[] AllSectorNames { get; }
		public static Dictionary<string, SectorAdditionalConfig> SectorsConfigs { get; }
		public static string BankAliveServiceAddress { get; }
		public static string BankAliveServiceEndpointName { get; }
		//public static string EncryptionKeyValue { get; set; }
	}
}
