using Common.Commanding;
using Common.ServiceInterfaces;
using System.ServiceModel;
using BankService.CommandingManager;
using System.Collections.Generic;
using System.Collections.Concurrent;
using BankService.Notification;
using Common;
using BankService.DatabaseManagement.Repositories;
using BankService.DatabaseManagement;
using System.Data.Entity;
using System.Threading;
using Common.Communication;
using System.Security;
using BankService.CommandExecutor;
using Common.Model;
using System;

namespace BankService
{
	/// <summary>
	/// Class represents banking service.
	/// </summary>
	[ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
	public class BankingService : IUserService, IAdminService
	{
		private readonly string commandingConnectionString = "CommandingDB";
		private readonly string bankDomainConnectionString = "BankServiceDB";
		private object locker = new object();

		private BankCommandingContext commandingContext;
		private BankDomainContext domainContext;

		private IAudit auditService; 
		private ICommandExecutor commandExecutor;
		private ICommandingManager commandManager;
		private INotificationHandler notificationHandler;

		private ConcurrentQueue<CommandNotification> responseQueue;
		public BankingService()
		{
			InitializesObjects();
			auditService = new AuditClientProxy(BankServiceConfig.AuditServiceAddress, BankServiceConfig.AuditServiceEndpointName);

			//FOR TESTING
			//TestCreateNewDatabase();

			responseQueue = new ConcurrentQueue<CommandNotification>();
			notificationHandler = new NotificationHandler(auditService, responseQueue, new NotificationContainer(ServiceLocator.GetService<IRepository<CommandNotification>>()));

			CommandQueue commandExecutorQueue = new CommandQueue(10, 0);

			commandManager = new CommandingManager.CommandingManager(auditService, commandExecutorQueue, responseQueue);

			commandExecutor = new CommandExecutor.CommandExecutor(auditService, commandExecutorQueue);

			

			commandExecutor.Start();
		}

		public void StartListeningForSectorConnections()
		{
			commandManager.StartListeningForConnectedSectors();
		}

		public void StartListeningForCheckAlivePings()
		{
			commandManager.StartListeningForAlivePings();
		}

		public CommandNotification CreateNewDatabase()
		{
			string username = StringFormatter.ParseName(Thread.CurrentPrincipal.Identity.Name);
			if (!Thread.CurrentPrincipal.IsInRole("admins"))
			{
				auditService.Log(username, "Failed authorization on creating new database command", System.Diagnostics.EventLogEntryType.Warning);
				throw new SecurityException("Access is denied.");
			}

			auditService.Log(username, "Authorized as admin, create data base access granted.", System.Diagnostics.EventLogEntryType.Warning);
			IClientServiceCallback callback = OperationContext.Current.GetCallbackChannel<IClientServiceCallback>();

			try
			{
				InternalCreateNewDatabase();
				CommandNotification cn = new CommandNotification(-1, CommandNotificationStatus.Confirmed);
				cn.Information = "Create new database.";
				return cn;
			}
			catch(Exception e)
			{
				CommandNotification cn = new CommandNotification(-1, CommandNotificationStatus.Rejected);
				cn.Information = "Couldn't create new database\n Reason: " + e.Message +".";
				return cn;
			}
		}

		public void InternalCreateNewDatabase()
		{
			if (commandingContext.Database.Exists())
			{
				commandingContext.Database.Connection.Close();
				commandingContext.Database.Delete();
			}

			commandingContext.Database.Create();
			commandingContext.Database.Connection.Open();
			auditService.Log(logMessage: "New BankCommandDB database created!", eventLogEntryType: System.Diagnostics.EventLogEntryType.Warning);

			if (domainContext.Database.Exists())
			{
				domainContext.Database.Connection.Close();
				domainContext.Database.Delete();
			}

			domainContext.Database.Create();
			domainContext.Database.Connection.Open();
			auditService.Log(logMessage: "New BankDomainDB database created!", eventLogEntryType: System.Diagnostics.EventLogEntryType.Warning);

			notificationHandler.ResetNotificationContainer(new NotificationContainer(ServiceLocator.GetService<IRepository<CommandNotification>>()));
		}

		public CommandNotification DeleteStaleCommands()
		{
			string username = StringFormatter.ParseName(Thread.CurrentPrincipal.Identity.Name);
			if (!Thread.CurrentPrincipal.IsInRole("admins"))
			{
				auditService.Log(username, "Failed authorization on deleting commands which are in timeout period.", System.Diagnostics.EventLogEntryType.Warning);
				throw new SecurityException("Access is denied.");
			}
			auditService.Log(username, "Authorized as admin, delete timeout command initiated.", System.Diagnostics.EventLogEntryType.Warning);
			IClientServiceCallback callback = OperationContext.Current.GetCallbackChannel<IClientServiceCallback>();

			try
			{
				commandManager.ClearStaleCommands();
				CommandNotification cn = new CommandNotification(-1, CommandNotificationStatus.Confirmed);
				cn.Information = "Remove expired commands.";
				return cn;

			}
			catch(Exception e)
			{
				CommandNotification cn = new CommandNotification(-1, CommandNotificationStatus.Rejected);
				cn.Information = "Remove expired commands.";
				return cn;
			}			
		}

		public void Deposit(double amount, long bankAccountId)
		{
			string username = StringFormatter.ParseName(Thread.CurrentPrincipal.Identity.Name);
			if (!Thread.CurrentPrincipal.IsInRole("users"))
			{
				auditService.Log(username, "Failed authorization on requesting deposit.", System.Diagnostics.EventLogEntryType.Warning);
				throw new SecurityException("Access is denied.");
			}

			auditService.Log(username, $"Authorized as user, {amount} requested.", System.Diagnostics.EventLogEntryType.Information);
			IClientServiceCallback callback = OperationContext.Current.GetCallbackChannel<IClientServiceCallback>();

			TransactionCommand depositCommand = new TransactionCommand(0, username, amount, TransactionType.Deposit, bankAccountId);
			long commandId = commandManager.EnqueueCommand(depositCommand);

			notificationHandler.RegisterCommand(username, callback, commandId);
		}

		public List<CommandNotification> GetPendingNotifications()
		{
			string username = StringFormatter.ParseName(Thread.CurrentPrincipal.Identity.Name);
			if (!Thread.CurrentPrincipal.IsInRole("users"))
			{
				auditService.Log(username, "Failed authorization on requesting pending notifications.", System.Diagnostics.EventLogEntryType.Warning);
				throw new SecurityException("Access is denied.");
			}

			auditService.Log(username, $"Authorized as user, asks for pending notifications.", System.Diagnostics.EventLogEntryType.Information);
			List<CommandNotification> userNotifications = notificationHandler.GetUserNotifications(username);

			return userNotifications;
		}

		public void Register()
		{
			string username = StringFormatter.ParseName(Thread.CurrentPrincipal.Identity.Name);
			if (!Thread.CurrentPrincipal.IsInRole("users"))
			{
				auditService.Log(username, "Failed authorization on register.", System.Diagnostics.EventLogEntryType.Warning);
				throw new SecurityException("Access is denied.");
			}

			auditService.Log(username, $"Authorized as user, asks for registration.", System.Diagnostics.EventLogEntryType.Information);
			IClientServiceCallback callback = OperationContext.Current.GetCallbackChannel<IClientServiceCallback>();

			RegistrationCommand registrationCommand = new RegistrationCommand(0, username);
			long commandId = commandManager.EnqueueCommand(registrationCommand);

			notificationHandler.RegisterCommand(username, callback, commandId);
		}

		public void RequestLoan(double amount, int months)
		{
			string username = StringFormatter.ParseName(Thread.CurrentPrincipal.Identity.Name);
			if (!Thread.CurrentPrincipal.IsInRole("users"))
			{
				auditService.Log(username, "Failed authorization on requesting loan.", System.Diagnostics.EventLogEntryType.Warning);
				throw new SecurityException("Access is denied.");
			}

			auditService.Log(username, $"Authorized as user, requests loan of {amount}.", System.Diagnostics.EventLogEntryType.Information);
			IClientServiceCallback callback = OperationContext.Current.GetCallbackChannel<IClientServiceCallback>();

			RequestLoanCommand requestLoanCommand = new RequestLoanCommand(0, username, amount, months);
			long commandId = commandManager.EnqueueCommand(requestLoanCommand);

			notificationHandler.RegisterCommand(username, callback, commandId);
		}

		public void Withdraw(double amount, long bankAccountId)
		{
			string username = StringFormatter.ParseName(Thread.CurrentPrincipal.Identity.Name);
			if (!Thread.CurrentPrincipal.IsInRole("users"))
			{
				auditService.Log(username, "Failed authorization on requesting withdraw.", System.Diagnostics.EventLogEntryType.Warning);
				throw new SecurityException("Access is denied.");
			}

			auditService.Log(username, $"Authorized as user, requests loan of {amount}.", System.Diagnostics.EventLogEntryType.Information);
			IClientServiceCallback callback = OperationContext.Current.GetCallbackChannel<IClientServiceCallback>();

			TransactionCommand withdrawCommand = new TransactionCommand(0, username, amount, TransactionType.Withdraw, bankAccountId);
			long commandId = commandManager.EnqueueCommand(withdrawCommand);

			notificationHandler.RegisterCommand(username, callback, commandId);
		}

		private void InitializesObjects()
		{
			commandingContext = new BankCommandingContext(commandingConnectionString);
			SemaphoreSlim commandingSemaphore = new SemaphoreSlim(1);
			ServiceLocator.RegisterObject<DbContext>(commandingContext);
			ServiceLocator.RegisterService<IRepository<BaseCommand>>(new Repository<BaseCommand>(commandingContext, commandingSemaphore));
			ServiceLocator.RegisterService<IRepository<CommandNotification>>(new Repository<CommandNotification>(commandingContext, commandingSemaphore));

			domainContext = new BankDomainContext(bankDomainConnectionString);
			ServiceLocator.RegisterObject<BankDomainContext>(domainContext);
		}

		public List<BankAccount> GetMyBankAccounts()
		{
			string username = StringFormatter.ParseName(Thread.CurrentPrincipal.Identity.Name);
			if (!Thread.CurrentPrincipal.IsInRole("users"))
			{
				auditService.Log(username, "Failed authorization on requesting bank accounts.", System.Diagnostics.EventLogEntryType.Warning);
				throw new SecurityException("Access is denied.");
			}

			List<BankAccount> bankAccounts = commandExecutor.GetUsersAccount(username);
			bankAccounts.ForEach(x => x.User = null);

			return bankAccounts;
		}
	}
}
