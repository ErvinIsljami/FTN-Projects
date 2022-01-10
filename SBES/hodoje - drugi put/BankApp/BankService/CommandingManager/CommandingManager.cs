using System;
using System.Collections.Generic;
using Common.Commanding;
using BankService.CommandingHost;
using System.Collections.Concurrent;
using BankService.DatabaseManagement;
using System.Data.Entity;
using BankService.DatabaseManagement.Repositories;
using Common;
using Common.Communication;
using Common.ServiceInterfaces;
using System.Linq;
using System.ServiceModel;
using System.Net.Security;
using System.Threading.Tasks;
using System.Threading;

namespace BankService.CommandingManager
{
	[ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
	public class CommandingManager : ICommandingManager, IDisposable, IStartupConfirmationService, IBankAliveService
	{
		private class CommandQueueResolver : IDisposable
		{
			private ConcurrentDictionary<string, Type> sectorToCommandMapper;
			private ConcurrentDictionary<Type, CommandQueue> commandToQueueMapper;

			public CommandQueueResolver()
			{
				// if configuration is changed, this initialization should be changed as well
				sectorToCommandMapper = new ConcurrentDictionary<string, Type>();
				sectorToCommandMapper.TryAdd(BankServiceConfig.AllSectorNames[2], typeof(TransactionCommand));
				sectorToCommandMapper.TryAdd(BankServiceConfig.AllSectorNames[1], typeof(RequestLoanCommand));
				sectorToCommandMapper.TryAdd(BankServiceConfig.AllSectorNames[0], typeof(RegistrationCommand));

				commandToQueueMapper = new ConcurrentDictionary<Type, CommandQueue>();
				commandToQueueMapper.TryAdd(typeof(TransactionCommand), new CommandQueue(BankServiceConfig.SectorQueueSize, BankServiceConfig.SectorQueueTimeoutInSeconds));
				commandToQueueMapper.TryAdd(typeof(RequestLoanCommand), new CommandQueue(BankServiceConfig.SectorQueueSize, BankServiceConfig.SectorQueueTimeoutInSeconds));
				commandToQueueMapper.TryAdd(typeof(RegistrationCommand), new CommandQueue(BankServiceConfig.SectorQueueSize, BankServiceConfig.SectorQueueTimeoutInSeconds));

			}

			public CommandQueue ResolveQueueForCommandType(Type commandType)
			{
				return commandToQueueMapper[commandType];
			}

			public CommandQueue ResolveQueueForSector(string sectorName)
			{
				return commandToQueueMapper[sectorToCommandMapper[sectorName]];
			}

			public void Dispose()
			{
				commandToQueueMapper.Clear();
				sectorToCommandMapper.Clear();
			}

			public List<CommandQueue> CommandQueues
			{
				get
				{
					return new List<CommandQueue>(commandToQueueMapper.Values);
				}
			}
		}

		private static readonly int queueSize;
		private static readonly int timeoutPeriod;
		private readonly DbContext dbContext;
		private ConcurrentQueue<CommandNotification> responseQueue;
		// Key is the Sector type name
		private Dictionary<string, ICommandingHost> commandingHosts;
		private IAudit auditService;
		private IDatabaseManager<BaseCommand> databaseManager;
		private ServiceHost startupConfirmationHost;
		private ServiceHost bankAliveServiceHost;
		private CommandQueueResolver commandQueueResolver;
		private CommandQueue commandExecutorQueue;

		static CommandingManager()
		{
			queueSize = BankServiceConfig.SectorQueueSize;
            timeoutPeriod = BankServiceConfig.SectorQueueTimeoutInSeconds;
		}

		public CommandingManager(IAudit auditService, CommandQueue commandExecutorQueue, ConcurrentQueue<CommandNotification> responseQueue)
		{
			dbContext = ServiceLocator.GetObject<DbContext>();
			this.commandExecutorQueue = commandExecutorQueue;
			this.auditService = auditService;
			this.responseQueue = responseQueue;

			commandQueueResolver = new CommandQueueResolver();

			InitialDatabaseLoading();

			commandingHosts = new Dictionary<string, ICommandingHost>();

			EnqueueNotSentCommands();

			PeriodicallyCheckSectorsAlive();
		}

		private void EnqueueNotSentCommands()
		{
			IEnumerable<BaseCommand> commands = databaseManager.Find(x => x.State == CommandState.NotSent);

			foreach (BaseCommand command in commands)
			{
				SendCommandToSpecificQueue(command);
				auditService.Log(logMessage: $"Command({command.GetType().Name} - id = {command.ID}) enqueued to commanding queue from database");
			}
		}

		public void ClearStaleCommands()
		{
			foreach (CommandQueue commandingQueue in commandQueueResolver.CommandQueues)
			{
				List<BaseCommand> expiredCommands = commandingQueue.ExpiredCommands();

				foreach (BaseCommand expiredCommand in expiredCommands)
				{
					commandingQueue.RemoveCommandById(expiredCommand.ID);
					databaseManager.RemoveEntity(expiredCommand.ID);
					auditService.Log(logMessage: $"Command(id = {expiredCommand.ID}) was in timeout period and is removed from commanding queue and database.");
				}
			}
		}

		public void CreateDatabase()
		{
			if (dbContext.Database.Exists())
			{
				dbContext.Database.Connection.Close();
				dbContext.Database.Delete();
			}

			dbContext.Database.Create();
            dbContext.Database.Connection.Open();
            auditService.Log(logMessage: "New BankCommandDB database created!", eventLogEntryType: System.Diagnostics.EventLogEntryType.Warning);
		}

		public void Dispose()
		{
			((IDisposable)databaseManager).Dispose();

			foreach (ICommandingHost commandingHost in commandingHosts.Values)
			{
				((IDisposable)commandingHost).Dispose();
			}

			commandQueueResolver.Dispose();
		}

		private void SendCommandToSpecificQueue(BaseCommand command)
		{
			commandQueueResolver.ResolveQueueForCommandType(command.GetType()).Enqueue(command);
		}

		public long EnqueueCommand(BaseCommand command)
		{
			databaseManager.AddEntity(command);

			auditService.Log("CommandManager", $"{command.GetType().Name}(id = {command.ID}) added to database!");

			SendCommandToSpecificQueue(command);

			return command.ID;

		}

		private void InitialDatabaseLoading()
		{
			try
			{
				dbContext.Database.Connection.Open();
				auditService.Log(logMessage: "Database connection opened.", eventLogEntryType: System.Diagnostics.EventLogEntryType.Information);
			}
			catch (Exception e)
			{
				auditService.Log(logMessage: "Database connection couldn't open. Reason = " + e.Message, eventLogEntryType: System.Diagnostics.EventLogEntryType.Error);
			}

			IRepository<BaseCommand> commandRepository = ServiceLocator.GetService<IRepository<BaseCommand>>();
			databaseManager = new DatabaseManager<BaseCommand>(commandRepository);
		}

		public void StartListeningForAlivePings()
		{
			bankAliveServiceHost = new ServiceHost(this);
			var binding = new NetTcpBinding();
			binding.CloseTimeout = binding.OpenTimeout = binding.ReceiveTimeout = binding.SendTimeout = new TimeSpan(1, 0, 0, 0);
			binding.Security.Mode = SecurityMode.Transport;
			binding.Security.Transport.ProtectionLevel = ProtectionLevel.Sign;
			binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
			var address = $"{BankServiceConfig.BankAliveServiceAddress}/{BankServiceConfig.BankAliveServiceEndpointName}";
			bankAliveServiceHost.AddServiceEndpoint(typeof(IBankAliveService), binding, address);
			try
			{
				bankAliveServiceHost.Open();
			}
			catch(Exception e)
			{
				throw;
			}
		}

		public void StartListeningForConnectedSectors()
		{
			startupConfirmationHost = new ServiceHost(this);
			var binding = new NetTcpBinding();
			binding.CloseTimeout = binding.OpenTimeout = binding.ReceiveTimeout = binding.SendTimeout = new TimeSpan(1, 0, 0, 0);
			binding.Security.Mode = SecurityMode.Transport;
			binding.Security.Transport.ProtectionLevel = ProtectionLevel.Sign;
			binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
			string address = $"{BankServiceConfig.StartupConfirmationServiceAddress}/{BankServiceConfig.StartupConfirmationServiceEndpointName}";
			startupConfirmationHost.AddServiceEndpoint(typeof(IStartupConfirmationService), binding, address);
			try
			{
				startupConfirmationHost.Open();
			}
			catch(Exception e)
			{
				throw;
			}
		}

		private void PeriodicallyCheckSectorsAlive()
		{
			Task.Run(() =>
			{
				while (true)
				{
					if(commandingHosts.Count > 0)
					{
						Dictionary<string, ICommandingHost> aliveSectors = new Dictionary<string, ICommandingHost>();
						Dictionary<string, ICommandingHost> copy = new Dictionary<string, ICommandingHost>();
						foreach(var pair in commandingHosts)
						{
							copy.Add(pair.Key, pair.Value);
						}

						foreach (var sectorHostPair in copy)
						{
							ConnectionInfo ci = BankServiceConfig.Connections[sectorHostPair.Key];
							using (var sectorServiceProxy = new WindowsClientProxy<ISectorService>(ci.Address, ci.EndpointName))
							{
								try
								{
									sectorServiceProxy.Proxy.CheckSectorAlive();
									aliveSectors.Add(sectorHostPair.Key, sectorHostPair.Value);
								}
								catch (Exception e)
								{
									sectorHostPair.Value.Dispose();
									Console.WriteLine($"Sector: {sectorHostPair.Key.ToUpper()} disconnected.");
									auditService.Log("CommandManager", $"Sector: {sectorHostPair.Key.ToUpper()} disconnected.", System.Diagnostics.EventLogEntryType.Information);
								}
							}
						}

						commandingHosts.Clear();
						foreach (var sectorHostPair in aliveSectors)
						{
							commandingHosts.Add(sectorHostPair.Key, sectorHostPair.Value);
						}
					}

					Thread.Sleep(1000);
				}
			});
		}

		public void OpenCommandHostForSector(string sectorType)
		{
			CommandQueue commandQueue = commandQueueResolver.ResolveQueueForSector(sectorType);
			CommandingHost.CommandingHost newHost = new CommandingHost.CommandingHost(sectorType, auditService, commandExecutorQueue, commandQueue, responseQueue, databaseManager);
			commandingHosts.Add(sectorType, newHost);
			newHost.Start();

			auditService.Log($"[CommandingHost({sectorType.ToUpper()})]", "Initialized.");
		}

		public void ConfirmStartup(string sectorType)
		{
			//Console.WriteLine($"{sectorType.ToUpper()} connected.");
			OpenCommandHostForSector(sectorType);
		}

		public void CheckIfBankAlive(string callingSector)
		{
			//Console.WriteLine($"{callingSector.ToUpper()} checking if I'm alive.");
		}
	}
}
