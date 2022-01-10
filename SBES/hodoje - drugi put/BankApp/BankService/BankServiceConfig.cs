using Common.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;

namespace BankService
{
	public struct ConnectionInfo
	{
		public string Address { get; set; }
		public string EndpointName { get; set; }
		public string SectorResponseAddress { get; set; }
		public string SectorResponseEndpoint { get; set; }
	}

	/// <summary>
	/// Loads app settings from App.config
	/// </summary>
	public static class BankServiceConfig
	{
		public const string BankServiceAddressConfigName = "BankServiceAddress";
		public const string SectorsConfigName = "Sectors";
		public const string SectorQueueSizeConfigName = "SectorQueueSize";
		public const string SectorQueueTimeoutInSecondsConfigName = "SectorQueueTimeoutInSeconds";
		public const string UserServiceEndpointNameConfigName = "UserServiceEndpointName";
		public const string AdminServiceEndpointNameConfigName = "AdminServiceEndpointName";
		public const string AuditServiceAddressConfigName = "AuditServiceAddress";
		public const string AuditServiceEndpointNameConfigName = "AuditServiceEndpointName";
		public const string AllSectorNamesConfigName = "AllSectorNames";
		public const string SectorExeFilenameConfigName = "SectorExeFilename";
		public const string StartupConfirmationServiceAddressConfigName = "StartupConfirmationServiceAddress";
		public const string StartupConfirmationServiceEndpointNameConfigName = "StartupConfirmationServiceEndpointName";
		public const string BankAliveServiceAddressConfigName = "BankAliveServiceAddress";
		public const string BankAliveServiceEndpointConfigName = "BankAliveServiceEndpointName";
		//public const string EncryptionKey = "EncryptionKey";

		static BankServiceConfig()
		{
			//EncryptionKeyValue = ConfigurationManager.AppSettings[EncryptionKey];
			BankAliveServiceAddress = ConfigurationManager.AppSettings[BankAliveServiceAddressConfigName];
			BankAliveServiceEndpointName = ConfigurationManager.AppSettings[BankAliveServiceEndpointConfigName];
			BankServiceAddress = ConfigurationManager.AppSettings[BankServiceAddressConfigName];
			UserServiceEndpointName = ConfigurationManager.AppSettings[UserServiceEndpointNameConfigName];
			AdminServiceEndpointName = ConfigurationManager.AppSettings[AdminServiceEndpointNameConfigName];
			try
			{
				SectorQueueSize = Int32.Parse(ConfigurationManager.AppSettings[SectorQueueSizeConfigName]);
				SectorQueueTimeoutInSeconds = Int32.Parse(ConfigurationManager.AppSettings[SectorQueueTimeoutInSecondsConfigName]);
			}
			catch(Exception e)
			{
				throw new Exception("Invalid configuration. Expected a number.", e);
			}

			AuditServiceAddress = ConfigurationManager.AppSettings[AuditServiceAddressConfigName];
			AuditServiceEndpointName = ConfigurationManager.AppSettings[AuditServiceEndpointNameConfigName];
			AllSectorNames = ConfigurationManager.AppSettings[AllSectorNamesConfigName].Split(',');
			string rawSectorExeFilename = ConfigurationManager.AppSettings[SectorExeFilenameConfigName];
			string workingDirectory = Environment.CurrentDirectory;
			string _solutionDirectory = Directory.GetParent(workingDirectory).Parent.Parent.FullName;
			SectorExeFilename = rawSectorExeFilename.Replace("{AppDir}", _solutionDirectory);
			StartupConfirmationServiceAddress = ConfigurationManager.AppSettings[StartupConfirmationServiceAddressConfigName];
			StartupConfirmationServiceEndpointName = ConfigurationManager.AppSettings[StartupConfirmationServiceEndpointNameConfigName];

			string sectorsConfigJson = ConfigurationManager.AppSettings[SectorsConfigName];
			SectorsConfigs = GetSectorsConfig(sectorsConfigJson);

			Connections = new Dictionary<string, ConnectionInfo>(SectorsConfigs.Count);
			foreach(var sectorName in AllSectorNames)
			{
				ConnectionInfo ci = new ConnectionInfo();
				ci.Address = SectorsConfigs[sectorName].Address;
				ci.EndpointName = SectorsConfigs[sectorName].EndpointName;
				ci.SectorResponseAddress = SectorsConfigs[sectorName].SectorResponseAddress;
				ci.SectorResponseEndpoint = SectorsConfigs[sectorName].SectorResponseEndpointName;
				Connections.Add(sectorName, ci);
			}
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

		public static string BankServiceAddress { get; }
		public static string UserServiceEndpointName { get; }
		public static string AuditServiceAddress { get; }
		public static string AuditServiceEndpointName { get; }
		public static Dictionary<string, ConnectionInfo> Connections { get; set; }
		public static string[] AllSectorNames { get; }
		public static Dictionary<string, SectorAdditionalConfig> SectorsConfigs { get; }
		public static int SectorQueueSize { get; }
		public static int SectorQueueTimeoutInSeconds { get; }
		public static string SectorExeFilename { get; }
		public static string StartupConfirmationServiceAddress { get; }
		public static string StartupConfirmationServiceEndpointName { get; }
		public static string AdminServiceEndpointName { get; }
		public static string BankAliveServiceAddress { get; }
		public static string BankAliveServiceEndpointName { get; }
		//public static string EncryptionKeyValue { get; set; }
	}
}
