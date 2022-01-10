using Common.Commanding;
using Common.Communication;
using Common.ServiceInterfaces;
using Common.SymmetricEncryptionAlgorithms;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SectorService
{
	/// <summary>
	/// Responsible for all the command processing and responding.
	/// </summary>
	public class SectorManager
	{
		#region Fields
		private CommandQueue _requestQueue;
		private CommandQueue _responseQueue;
		private WindowsClientProxy<ISectorResponseService> _responseProxy;
		private Task _processorTask;
		private Task _responderTask;
		private ISymmetricAlgorithmProvider symmetricAlgorithm;
		private EncryptionInformation encryptionInfo;
		#endregion

		#region Properties
		#endregion

		#region Constructors
		/// <summary>
		/// Initializes an instance of <see cref="SectorManager" class./>
		/// </summary>
		public SectorManager(string sectorType, int sectorQueueSize, int sectorQueueTimeoutPeriodInSeconds)
		{
			encryptionInfo = new EncryptionInformation()
			{
				Key = " ?$&>?e?`d??[??????M<$H??????",
				CipherMode = System.Security.Cryptography.CipherMode.CBC
			};

			symmetricAlgorithm = new AESAlgorithmProvider();
			_requestQueue = new CommandQueue(sectorQueueSize, sectorQueueTimeoutPeriodInSeconds);
			_responseQueue = new CommandQueue(sectorQueueSize, sectorQueueTimeoutPeriodInSeconds);
			_responseProxy = new WindowsClientProxy<ISectorResponseService>(
				SectorConfig.SectorsConfigs[sectorType].SectorResponseAddress, SectorConfig.SectorsConfigs[sectorType].SectorResponseEndpointName);
			_processorTask = new Task(ProcessCommands);
			_responderTask = new Task(SendResponses);

			_processorTask.Start();
			_responderTask.Start();
		}
		#endregion

		#region Methods
		/// <summary>
		/// Enqueues a command for processing. Used by Sector service to enqueue a command recieved from Bank service.
		/// </summary>
		/// <param name="command">Command recieved from Bank service.</param>
		public void EnqueueCommand(BaseCommand command)
		{
			_requestQueue.Enqueue(command);
		}

		/// <summary>
		/// Confirms a given command to Bank service.
		/// </summary>
		/// <param name="commandId">Id of a command that has been successfully executed.</param>
		private void AcceptRequest(long commandId, string information)
		{
			try
			{
				byte[] rawData = BitConverter.GetBytes(commandId).Concat(Encoding.ASCII.GetBytes(information)).ToArray();

				_responseProxy.Proxy.Accept(commandId, information, symmetricAlgorithm.Encrypt(encryptionInfo, rawData));
			}
			catch(Exception e)
			{
				throw;
			}
		}

		/// <summary>
		/// Rejects a given command to Bank service.
		/// </summary>
		/// <param name="commandId">Id of a command that has been rejected.</param>
		/// <param name="reason">Reason why a command has been rejected.</param>
		private void RejectRequest(long commandId, string reason)
		{
			try
			{
				byte[] rawData = BitConverter.GetBytes(commandId).Concat(Encoding.ASCII.GetBytes(reason)).ToArray();

				_responseProxy.Proxy.Reject(commandId, reason, symmetricAlgorithm.Encrypt(encryptionInfo, rawData));
			}
			catch(Exception e)
			{
				throw;
			}
		}
		
		/// <summary>
		/// Processes arrived requests and puts them in response queue.
		/// </summary>
		private void ProcessCommands()
		{
			while (true)
			{
				BaseCommand command;
				try
				{
					command = _requestQueue.Dequeue();
				}
				catch(Exception e)
				{
					throw;
				}
				if (command.TimedOut)
				{
					command.Status = CommandNotificationStatus.Rejected;
				}
				else
				{
					while (true)
					{
						string shouldAccept = GetSectorWorkerInput(command);

						if (shouldAccept == "y")
						{
							command.Status = CommandNotificationStatus.Confirmed;
							break;
						}
						else if(shouldAccept == "n")
						{
							command.Status = CommandNotificationStatus.Rejected;
							break;
						}
						else
						{
							continue;
						}
					}
				}
				_responseQueue.Enqueue(command);
			}
		}

		/// <summary>
		/// Takes commands from response queue and sends according responses to Bank service.
		/// </summary>
		private void SendResponses()
		{
			while (true)
			{
				BaseCommand command = _responseQueue.Dequeue();
				switch (command.Status)
				{
					case CommandNotificationStatus.Confirmed:
						AcceptRequest(command.ID, command.StringifyCommand());
						break;
					case CommandNotificationStatus.Rejected:
						RejectRequest(command.ID, command.StringifyCommand());
						break;
					case CommandNotificationStatus.None:
						RejectRequest(command.ID, command.StringifyCommand());
						break;
					default:
						RejectRequest(command.ID, "Invalid request.");
						break;
				}
			}
		}

		/// <summary>
		/// Gets the Sector worker input about if the command should be accepted or rejected.
		/// </summary>
		/// <param name="command">Received command.</param>
		/// <returns>Returns the user input either 'y' for accpet or 'n' for reject.</returns>
		private string GetSectorWorkerInput(BaseCommand command)
		{
			Console.Write($"{command.StringifyCommand()}{Environment.NewLine}Type 'y' for accept and 'n' for reject: ");
			Console.ForegroundColor = ConsoleColor.DarkGreen;
			string input = Console.ReadLine();
			Console.ForegroundColor = ConsoleColor.Gray;
			return input;
		}
		#endregion
	}
}
