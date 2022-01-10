using Common.Communication;
using Common.Model;
using Common.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SectorService.ServiceHosts
{
	public class SectorServiceServiceHost : IDisposable
	{
		private readonly string _sectorServiceAddress;
		private readonly string _sectorServiceEndpointName;
		private readonly ServiceHost _sectorServiceServiceHost;
		private readonly NetTcpBinding _binding;
		private WindowsClientProxy<IStartupConfirmationService> _startupProxy;
		private WindowsClientProxy<IBankAliveService> _bankALiveServiceProxy;
		private readonly string _sectorType;

		public SectorServiceServiceHost(string sectorType, SectorAdditionalConfig sectorConfig)
		{
			_sectorType = sectorType;
			_sectorServiceAddress = sectorConfig.Address;
			_sectorServiceEndpointName = sectorConfig.EndpointName;
			_binding = SetUpBinding();
			Services.SectorService sectorService = new Services.SectorService(sectorType, SectorConfig.SectorQueueSize, SectorConfig.SectorQueueTimeoutInSeconds);
			_sectorServiceServiceHost = new ServiceHost(sectorService);
			_sectorServiceServiceHost.AddServiceEndpoint(typeof(ISectorService), _binding,
				$"{_sectorServiceAddress}/{_sectorServiceEndpointName}");
		}

		private void CheckIfBankAlive()
		{
			Task.Run(() =>
			{
				while (true)
				{
					try
					{
						//Console.WriteLine("Check if bank alive...");
						_bankALiveServiceProxy.Proxy.CheckIfBankAlive(_sectorType);
						//Console.WriteLine("Bank alive.");
					}
					catch (Exception e)
					{
						//Console.WriteLine("Bank disconnected.");
						_bankALiveServiceProxy = null;
						TryConnectToBank();
						return;
					}
					Thread.Sleep(1000);
				}
			});
		}

		public void TryConnectToBank()
		{
			bool isSectorConfirmed = false;

			while (!isSectorConfirmed)
			{
				try
				{
					//Console.WriteLine("Trying to connect to bank...");
					_startupProxy = new WindowsClientProxy<IStartupConfirmationService>(
						SectorConfig.StartupConfirmationServiceAddress, SectorConfig.StartupConfirmationServiceEndpointName);
					try
					{
						_startupProxy.Proxy.ConfirmStartup(_sectorType);
						isSectorConfirmed = true;
						//Console.WriteLine("Connected.");
						_bankALiveServiceProxy = new WindowsClientProxy<IBankAliveService>(SectorConfig.BankAliveServiceAddress, SectorConfig.BankAliveServiceEndpointName);
						CheckIfBankAlive();
						break;
					}
					catch(Exception e)
					{
						_startupProxy = null;
						int sleep = 5;
						//Console.WriteLine("Bank not available.");
						//Console.WriteLine($"Trying again in {sleep} seconds.");
						isSectorConfirmed = false;
					}
				}
				catch (Exception e)
				{
					_startupProxy = null;
					int sleep = 1;
					//Console.WriteLine("Bank not available.");
					//Console.WriteLine($"Trying again in {sleep} seconds.");
					isSectorConfirmed = false;
					Thread.Sleep(sleep * 1000);
				}
			}
		}

		public void OpenService()
		{
			try
			{
				_sectorServiceServiceHost.Open();
			}
			catch (Exception ex)
			{
				Console.WriteLine($"{_sectorServiceEndpointName.ToUpper()} ServiceHost failed to open with an error: {ex.Message}");
			}
		}

		public void CloseService()
		{
			try
			{
				_sectorServiceServiceHost.Close();
			}
			catch (Exception ex)
			{
				Console.WriteLine($"{_sectorServiceEndpointName.ToUpper()} ServiceHost failed to close with an error: {ex.Message}");
			}
		}

		private NetTcpBinding SetUpBinding()
		{
			var binding = new NetTcpBinding();
			binding.CloseTimeout = binding.OpenTimeout = binding.ReceiveTimeout = binding.SendTimeout = new TimeSpan(1, 0, 0, 0);
			binding.Security.Mode = SecurityMode.Transport;
			binding.Security.Transport.ProtectionLevel = ProtectionLevel.Sign;
			binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
			return binding;
		}

		public void Dispose()
		{
			(_sectorServiceServiceHost as IDisposable).Dispose();
		}
	}
}
