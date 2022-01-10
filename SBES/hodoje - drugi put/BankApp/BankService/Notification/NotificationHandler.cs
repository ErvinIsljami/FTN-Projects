using Common.Commanding;
using Common.ServiceInterfaces;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Collections.Generic;
using BankService.Notification;
using Common.Communication;
using Common.DataContracts.Dtos;

namespace BankService
{
	public class NotificationHandler : INotificationHandler, IDisposable
	{
		private CancellationTokenSource cancellationToken;
		private INotificationContainer notificationContainer;
		private ConcurrentQueue<CommandNotification> notificationQueue;
		private IAudit auditService;

		public NotificationHandler(IAudit auditService, ConcurrentQueue<CommandNotification> notificationQueue, INotificationContainer notificationContainer)
		{
			this.auditService = auditService;
			this.notificationQueue = notificationQueue;
			this.notificationContainer = notificationContainer;

			cancellationToken = new CancellationTokenSource();

			Task listenQueueTask = new Task(ListenForCommandNotifications);
			listenQueueTask.Start();
		}

		public void Dispose()
		{
			Stop();
		}

		public List<CommandNotification> GetUserNotifications(string username)
		{
			List<CommandNotification> notifications = notificationContainer.GetCommandNotificationsForUser(username);

			return notifications?? new List<CommandNotification>(0);
		}

		public void RegisterCommand(string username, IClientServiceCallback userCallback, long commandId)
		{
			notificationContainer.AddExpectingNotificationId(username, userCallback, commandId);
			auditService.Log("NotificationHandler", $"Added expected notification(id={commandId}) in container.", System.Diagnostics.EventLogEntryType.Information);
		}

		public void Start()
		{
			Task listenWorker = new Task(ListenForCommandNotifications);
			listenWorker.Start();
		}

		public void Stop()
		{
			cancellationToken.Cancel();
		}

		private void ListenForCommandNotifications()
		{
			while (!cancellationToken.IsCancellationRequested)
			{
				CommandNotification notification;
				if (!notificationQueue.TryDequeue(out notification))
				{
					Thread.Sleep(2000);
					continue;
				}

				auditService.Log("NotificationHandler", $"Received notification with id: {notification.ID}");

				string username;
				IClientServiceCallback callback = notificationContainer.CommandNotificationReceived(notification, out username);

                if (callback == null)
                {
                    continue;
                }

				if (SendNotificationToClient(callback, notification))
				{
					notificationContainer.CommandNotificationSent(notification.ID);
				}
				else
				{
					// set callback on null
					notificationContainer.DefaultUsersCallback(username);
				}
			}
		}

		private bool SendNotificationToClient(IClientServiceCallback callback, CommandNotification commandNotification)
		{
			try
			{
				callback.SendNotification(commandNotification);
				auditService.Log(logMessage: $"Command notification sent to client(id={commandNotification.ID}).", eventLogEntryType: System.Diagnostics.EventLogEntryType.Information);
				return true;
			}
			catch
			{
				return false;
			}
		}

		public void ResetNotificationContainer(INotificationContainer notificationContainer)
		{
			this.notificationContainer = notificationContainer;
		}
	}
}
