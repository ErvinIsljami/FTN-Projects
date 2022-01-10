using Client.Model;
using Common.Commanding;
using Common.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
	public class BankServiceCallbackObject : IClientServiceCallback
	{
		private Action<CommandNotification> _callback;
		private Action getBankAccounts;

		public BankServiceCallbackObject(Action<CommandNotification> callback, Action getBankAccounts)
		{
			_callback = callback;
			//this.getBankAccounts = getBankAccounts;
		}

		public void SendNotification(CommandNotification commandNotification)
		{
			_callback(commandNotification);
			//if (commandNotification.CommandStatus == CommandNotificationStatus.Confirmed)
			//{
			//	getBankAccounts?.Invoke();
			//}
		}
	}
}
