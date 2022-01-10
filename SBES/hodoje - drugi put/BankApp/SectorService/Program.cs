using Common.Model;
using SectorService.ServiceHosts;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading;

namespace SectorService
{
	class Program
	{
		static void Main(string[] args)
		{
			Dictionary<string, string> sectorTypes = new Dictionary<string, string>
			{
				["0"] = "accountSector",
				["1"] = "loanSector",
				["2"] = "transactionSector"
			};
			string sectorType = "";
			while (!sectorTypes.ContainsKey(sectorType))
			{
				Console.WriteLine("Choose what sector you want to run:");
				foreach(var sectorPair in sectorTypes)
				{
					Console.WriteLine($"[{sectorPair.Key}] - {sectorPair.Value.ToUpper()}");
				}
				sectorType = Console.ReadLine();
			}

			string chosenSectorType = sectorTypes[sectorType];

			Console.WriteLine($"Starting {chosenSectorType.ToUpper()}...");

			try
			{
				SectorAdditionalConfig sectorAdditionalConfig;
				if (SectorConfig.SectorsConfigs.TryGetValue(chosenSectorType, out sectorAdditionalConfig))
				{
					var sectorHost = new SectorServiceServiceHost(chosenSectorType, sectorAdditionalConfig);
					sectorHost.OpenService();
					sectorHost.TryConnectToBank();
					Console.WriteLine($"{chosenSectorType.ToUpper()} started.");
				}
				else
				{
					throw new Exception($"{chosenSectorType.ToUpper()} ServiceHost does not exist.");
				}
			}
			catch (Exception e)
			{
				Console.WriteLine("Reporting exception to Bank service.");
				Console.WriteLine("Shutting down...");
				throw new FaultException(e.Message);
			}

			// Don't let me go
			SemaphoreSlim ss = new SemaphoreSlim(0);
			ss.Wait();
		}
	}
}
