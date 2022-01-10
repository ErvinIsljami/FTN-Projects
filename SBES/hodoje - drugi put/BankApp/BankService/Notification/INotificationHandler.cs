using BankService.Notification;
using Common.Commanding;
using Common.Model;
using Common.ServiceInterfaces;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace BankService
{
	public interface INotificationHandler
	{
		List<CommandNotification> GetUserNotifications(string key);
		void RegisterCommand(string username, IClientServiceCallback userCallback, long commandId);
		void Start();
		void Stop();
		void ResetNotificationContainer(INotificationContainer notificationContainer);
	}

	public class NotificationInformation : IdentifiedObject
	{
		public NotificationInformation(string username, IClientServiceCallback userCallback = null, List<CommandNotification> readyNotifications = null)
		{
			Username = username;
			UserCallback = userCallback;
			ReadyNotifications = readyNotifications != null ? new ConcurrentDictionary<long, CommandNotification>(readyNotifications?.ToDictionary(x => x.ID, y => y)) : new ConcurrentDictionary<long, CommandNotification>();
			PendingNotifications = new ConcurrentDictionary<long, CommandNotification>();
		}

		public ConcurrentDictionary<long, CommandNotification> ReadyNotifications { get; private set; }
		public ConcurrentDictionary<long, CommandNotification> PendingNotifications { get; private set; }
		public IClientServiceCallback UserCallback { get; set; }
		public string Username { get; private set; }

		public override int GetHashCode()
		{
			return Username.GetHashCode();
		}
	}
}
