using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Common.Commanding
{
	/// <summary>
	/// Class which represents commanding queue.
	/// </summary>
	public class CommandQueue : IDisposable
	{
		private List<BaseCommand> commandingQueue;
		private AutoResetEvent collectionSynchronication;
		private object locker;
		private TimeSpan timeoutPeriod;

		/// <summary>
		/// Initializes new instance of <see cref="CommandQueue"/> class. 
		/// </summary>
		/// <param name="queueSize">Predefined queue size.</param>
		/// <param name="timeoutPeriodInSeconds">Timeout period in seconds.</param>
		public CommandQueue(int queueSize, int timeoutPeriodInSeconds)
		{
			commandingQueue = new List<BaseCommand>(queueSize);
			locker = new object();
			collectionSynchronication = new AutoResetEvent(false);
			timeoutPeriod = new TimeSpan(0, 0, timeoutPeriodInSeconds);
		}

		/// <summary>
		/// Adds command to the queue.
		/// </summary>
		/// <param name="command">Command to add.</param>
		public void Enqueue(BaseCommand command)
		{
			lock (locker)
			{
				commandingQueue.Add(command);
				collectionSynchronication.Set();
			}
		}

		/// <summary>
		/// Returns commands in <b>FIFO</b> order.
		/// </summary>
		/// <returns>Returns command if there is any in the collection, otherwise null.</returns>
		public BaseCommand Dequeue()
		{
			BaseCommand returnCommand = null;
			int commandingQueueSize = 0;

			lock (locker)
			{
				commandingQueueSize = commandingQueue.Count;
			}

			if (commandingQueueSize == 0)
			{
				collectionSynchronication.WaitOne();
			}

			lock (locker)
			{
				if (commandingQueue.Count > 0)
				{
					returnCommand = commandingQueue[0];
					commandingQueue.RemoveAt(0);
				}
			}

			return returnCommand;
		}

		/// <summary>
		/// Returns the command within queue with given command ID.
		/// </summary>
		/// <param name="commandId">Command id to search for.</param>
		/// <returns>Command if command exists in the queue, otherwise null.</returns>
		public BaseCommand GetCommandById(long commandId)
		{
			BaseCommand command;

			lock (locker)
			{
				command = commandingQueue.FirstOrDefault(x => x.ID == commandId);
			}

			return command;
		}

		/// <summary>
		/// Removes command if it exists in the queue.
		/// </summary>
		/// <param name="commandId">Command id of the command to be removed.</param>
		/// <returns>Returns <b>true</b> if command is removed, otherwise <b>false</b>.</returns>
		public bool RemoveCommandById(long commandId)
		{
			BaseCommand command;
			lock (locker)
			{
				command = commandingQueue.FirstOrDefault(x => x.ID == commandId);
			}

			if (command == null)
			{
				return false;
			}

			lock (locker)
			{
				commandingQueue.Remove(command);
			}

			return true;
		}

		/// <summary>
		/// Determines commands which timeout period has expired.
		/// </summary>
		/// <returns>Returns commands which are in timeout or empty list if there are none.</returns>
		public List<BaseCommand> ExpiredCommands()
		{
			List<BaseCommand> expiredCommands = new List<BaseCommand>();

			lock (locker)
			{
				DateTime dateTimeNow = DateTime.Now;

				foreach (BaseCommand command in commandingQueue)
				{
					if (command.CreationTime + timeoutPeriod <= dateTimeNow)
					{
						command.TimedOut = true;
						expiredCommands.Add(command);
					}
				}
			}

			return expiredCommands;
		}

		/// <inheritdoc/>
		public void Dispose()
		{
			commandingQueue.Clear();
			collectionSynchronication.Set();
		}
	}
}
