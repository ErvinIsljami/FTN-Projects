using System;
using System.Threading.Tasks;
using System.Threading;
using Common.Commanding;
using BankService.CommandHandler;
using System.Collections.Concurrent;
using BankService.DatabaseManagement;
using Common.Communication;
using Common.ServiceInterfaces;
using System.Collections.Generic;
using System.Linq;

namespace BankService.CommandingHost
{
	public class CommandingHost : ICommandingHost, INotificationHost
	{
		private string sectorType;

		private CancellationTokenSource cancellationToken = new CancellationTokenSource();
		private AutoResetEvent sendingSynchronization = new AutoResetEvent(false);

		private CommandQueue commandingQueue;
		private CommandQueue commandExecutionerQueue;
		private ConcurrentQueue<CommandNotification> responseQueue;

		private ICommandHandler commandHandler;
		private IDatabaseManager<BaseCommand> databaseManager;

		private IAudit auditService;

		public CommandingHost(string sectorType, IAudit auditService, CommandQueue commandExecutionerQueue, CommandQueue commandingQueue, ConcurrentQueue<CommandNotification> responseQueue, IDatabaseManager<BaseCommand> databaseManager)
		{
			this.commandExecutionerQueue = commandExecutionerQueue;
			this.databaseManager = databaseManager;
			this.auditService = auditService;
			this.responseQueue = responseQueue;
			this.commandingQueue = commandingQueue;
			this.sectorType = sectorType;

			commandHandler = new CommandHandler.CommandHandler(sectorType, auditService, this, BankServiceConfig.SectorQueueSize, LoadSentCommands());
		}

		public void CommandNotificationReceived(CommandNotification commandNotification)
		{
			responseQueue.Enqueue(commandNotification);

			BaseCommand command = databaseManager.Get(commandNotification.ID);
			if (command != null)
			{
				command.State = CommandState.Executed;
				command.Status = commandNotification.CommandStatus;
				databaseManager.Update(command);

				auditService.Log(command.ToString(), "Changed state to executed!");

				if (command.Status == CommandNotificationStatus.Confirmed)
				{
					commandExecutionerQueue.Enqueue(command);
				}
			}

			// Awake WorkerThread because there is enough command space in Commanding Handler.
			sendingSynchronization.Set();
		}

		public void Dispose()
		{
			Stop();
			commandHandler.Dispose();
		}

		public void Start()
		{
			// Get commands from commanding queue and send it to Commanding Handler.
			Task listenQueueTask = new Task(WorkerThread);
			listenQueueTask.Start();
		}

		public void Stop()
		{
			// Cancel WorkerThread
			cancellationToken?.Cancel();

			// Wake up WorkerThread if it is waiting for Commanding Handler to get enough space.
			sendingSynchronization?.Set();
		}

		private void WorkerThread()
		{
			Console.WriteLine($"[CommandingHost({sectorType.ToUpper()})] started...");
			while (!cancellationToken.IsCancellationRequested)
			{
				Console.WriteLine("Worker thread working...");
				BaseCommand commandToSend = commandingQueue.Dequeue();
				Console.WriteLine("Processing command...");
				// Queue might be in disposing procedure.
				if (commandToSend == null)
				{
					continue;
				}

				auditService.Log("CommandingHost", $"Dequeued {commandToSend.GetType().Name}(id = {commandToSend.ID}) command.");

				//if (!commandHandler.HasAvailableSpace())
				//{
				//	// If there is not enough space on host (sector is full) wait for response to be received.
				//	sendingSynchronization.WaitOne();
				//}

				if (cancellationToken.IsCancellationRequested)
				{
					// When awoken, check if cancellation token was canceled (object disposing).
					return;
				}

				if (commandHandler.SendCommandToSector(commandToSend))
				{
					ChangeCommandState(commandToSend.ID, CommandState.Sent);
					sendingSynchronization.WaitOne();
				}
				// command not sent
				else
				{
                    // If command handler couldn't sent the command, requeue the command.
					commandingQueue.Enqueue(commandToSend);
				}
			}
		}

		private List<BaseCommand> LoadSentCommands()
		{
			return databaseManager.Find(x => x.State == CommandState.Sent).ToList();
		}

		private void ChangeCommandState(long id, CommandState state)
		{
			BaseCommand command = databaseManager.Get(id);
			if (command == null)
			{
				return;
			}

			command.State = state;

			// log to audit : command changed state
			auditService.Log("CommandHandler", $"Command ({command.ToString()}) changed state to sent.");

			databaseManager.Update(command);
		}
	}
}
