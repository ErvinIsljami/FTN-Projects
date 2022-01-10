using Common.Commanding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Model
{
	public class Notification : BindableBase
	{
		private string _message;
		private string _displayedInfo;
		private CommandNotificationStatus _status;

		public string Message
		{
			get { return _message; }
			set { SetField(ref _message, value); }
		}

		public string DisplayedInfo
		{
			get { return _displayedInfo; }
			set { SetField(ref _displayedInfo, value); }
		}

		public CommandNotificationStatus Status
		{
			get { return _status; }
			set { SetField(ref _status, value); }
		}

		public Notification(string message, CommandNotificationStatus status)
		{
			_message = message;
			_status = status;
			DisplayedInfo = $"{Message}{Environment.NewLine}Status: {status}";
		}

		public override string ToString()
		{
			return $"{Message}{Environment.NewLine}Status: {Status}";
		}
	}
}
