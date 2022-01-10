namespace Common.Commanding
{
	/// <summary>
	/// Indicates command state.
	/// </summary>
	public enum CommandState
	{
		NotSent, // Command is not sent.
		Sent, // Command is sent.
		Executed // Command response received.
	}

	/// <summary>
	/// Indicates the current state of the notification.
	/// </summary>
	public enum NotificationState
	{
		Expected,
		Received,
		Sent
	}

	/// <summary>
	/// Indicates status of the command.
	/// </summary>
	public enum CommandNotificationStatus
	{
		None, // Used when command is not in executed state
		Confirmed, // Command was successfully executed.
		Rejected // Command was rejected.
	}
}
