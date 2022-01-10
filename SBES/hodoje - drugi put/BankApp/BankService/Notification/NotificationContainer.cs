using BankService.DatabaseManagement.Repositories;
using Common.Commanding;
using Common.ServiceInterfaces;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace BankService.Notification
{
	public class NotificationContainer : INotificationContainer
	{
		private IRepository<CommandNotification> dataPersister;
		private ConcurrentDictionary<string, NotificationInformation> pendingUserNotifications;

		public NotificationContainer(IRepository<CommandNotification> dataPersister)
		{
			this.dataPersister = dataPersister;
			pendingUserNotifications = new ConcurrentDictionary<string, NotificationInformation>(LoadNotificationsFromDatabase().ToDictionary(x => x.Username, x => x));
		}

		public void AddExpectingNotificationId(string username, IClientServiceCallback userCallback, long commandId)
		{
			CommandNotification notification = new CommandNotification(commandId);
			notification.Username = username;
			NotificationInformation userNotificationInfo = pendingUserNotifications.GetOrAdd(username, (x) => { return new NotificationInformation(username, userCallback); });
			userNotificationInfo.PendingNotifications.TryAdd(commandId, notification);

			dataPersister.AddEntity(notification);
		}

		public IClientServiceCallback CommandNotificationReceived(CommandNotification receivedCommandNotification, out string username)
		{
			username = "";
			foreach (NotificationInformation notificationinfo in pendingUserNotifications.Values)
			{
				CommandNotification notification;
				if (notificationinfo.PendingNotifications.TryRemove(receivedCommandNotification.ID, out notification))
				{
					notification.NotificationState = NotificationState.Received;
                    notification.Information = receivedCommandNotification.Information;
					notification.CommandStatus = receivedCommandNotification.CommandStatus;
					CommandNotification dbNotification = dataPersister.Get(notification.ID);
					if (dbNotification != null)
					{
						dbNotification.NotificationState = notification.NotificationState;
						dbNotification.Information = notification.Information;
						dbNotification.CommandStatus = receivedCommandNotification.CommandStatus;
						dataPersister.Update(notification);
					}

					notificationinfo.ReadyNotifications.TryAdd(notification.ID, notification);

					username = notificationinfo.Username;

					return notificationinfo.UserCallback;
				}
			}

			return null;
		}

		public void DefaultUsersCallback(string username)
		{
			NotificationInformation notificationInfo;
			if (pendingUserNotifications.TryGetValue(username, out notificationInfo))
			{
				notificationInfo.UserCallback = null;
			}
		}

		public void CommandNotificationSent(long notificationId)
		{
			foreach (NotificationInformation notificationinfo in pendingUserNotifications.Values)
			{
				CommandNotification commandNotification;
				if (notificationinfo.ReadyNotifications.TryRemove(notificationId, out commandNotification))
				{
					CommandNotification notification = dataPersister.Get(notificationId);

					if (notification == null)
					{
						return;
					}

					notification.NotificationState = NotificationState.Sent;

					dataPersister.Update(notification);
				}
			}
		}

		public List<CommandNotification> GetCommandNotificationsForUser(string username)
		{
			NotificationInformation userNotification;
			List<CommandNotification> userNotifications = new List<CommandNotification>(0);
			if (pendingUserNotifications.TryRemove(username, out userNotification))
			{

				userNotifications = userNotification?.ReadyNotifications.Values.ToList();
				foreach (CommandNotification commandNotification in userNotifications)
				{
					CommandNotification dbNotification = dataPersister.Get(commandNotification.ID);
					dbNotification.NotificationState = NotificationState.Sent;

					dataPersister.Update(dbNotification);
				}
			}

			return userNotifications;
		}

		private List<NotificationInformation> LoadNotificationsFromDatabase()
		{
			IEnumerable<CommandNotification> notifications = dataPersister.Find(x => x.NotificationState != NotificationState.Sent);

			List<NotificationInformation> notificationInfos = new List<NotificationInformation>(notifications.Count());

			foreach (var pair in notifications.GroupBy(x => x.Username))
			{
				NotificationInformation notificationInfo = new NotificationInformation(pair.Key, readyNotifications: pair.ToList());
				notificationInfos.Add(notificationInfo);
			}

			return notificationInfos;
		}
	}
}