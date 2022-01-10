using Common.Commanding;
using Common.ServiceInterfaces;
using System.Collections.Generic;

namespace BankService.Notification
{
	public interface INotificationContainer
	{
		void DefaultUsersCallback(string username);
		void CommandNotificationSent(long commandId);

		void AddExpectingNotificationId(string username, IClientServiceCallback userCallback, long commandId);

		List<CommandNotification> GetCommandNotificationsForUser(string username);

		IClientServiceCallback CommandNotificationReceived(CommandNotification receivedCommandNotification, out string username);
	}
}
