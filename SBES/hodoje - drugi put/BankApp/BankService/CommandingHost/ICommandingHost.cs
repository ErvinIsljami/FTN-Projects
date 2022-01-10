using BankService.DatabaseManagement;
using Common.Commanding;
using System;

namespace BankService.CommandingHost
{
	/// <summary>
	/// Interface used for unit which has logic for sending commands and propagating responses to client.
	/// </summary>
	public interface ICommandingHost : IDisposable
	{
		/// <summary>
		/// Commanding host starts getting commands from queue to execute and send them to command handler.
		/// </summary>
		void Start();

		/// <summary>
		/// Stops the commanding host.
		/// </summary>
		void Stop();
	}

	public interface INotificationHost
	{
		/// <summary>
		/// Notify host that a response has been received.
		/// </summary>
		/// <param name="commandNotification">Command notification received.</param>
		void CommandNotificationReceived(CommandNotification commandNotification);
	}
}
