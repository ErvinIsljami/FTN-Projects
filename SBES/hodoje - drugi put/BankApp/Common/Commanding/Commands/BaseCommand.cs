using Common.Model;
using System;
using System.Runtime.Serialization;

namespace Common.Commanding
{
	/// <summary>
	/// Class used to represent user command.
	/// </summary>
	[DataContract]
	[KnownType(typeof(TransactionCommand))]
	[KnownType(typeof(RequestLoanCommand))]
	[KnownType(typeof(RegistrationCommand))]
	[Serializable]
	public abstract class BaseCommand : IdentifiedObject
	{
		public BaseCommand()
		{

		}

		/// <summary>
		/// Initializes new instance of <see cref="BaseCommand"/> class.
		/// </summary>
		/// <param name="commandId">Unique command id.</param>
		public BaseCommand(long commandId, string username) : base(commandId)
		{
			CreationTime = DateTime.Now;
			Status = CommandNotificationStatus.None;
			State = CommandState.NotSent;
			Username = username;
		}

		/// <summary>
		/// Defines when was the command created.
		/// </summary>
		[DataMember]
		public DateTime CreationTime { get; private set; }

		/// <summary>
		/// Command status determined by Sector.
		/// </summary>
		[DataMember]
		public CommandNotificationStatus Status { get; set; }

		/// <summary>
		/// Command state in 
		/// </summary>
		[DataMember]
		public CommandState State { get; set; }

		/// <summary>
		/// Username of the user who requested withdraw.
		/// </summary>
		[DataMember]
		public string Username { get; private set; }

		/// <summary>
		/// Indicates if command is in timeout.
		/// </summary>
		public bool TimedOut { get; set; }

		public abstract string StringifyCommand();

		/// <inheritdoc/>
		public override bool Equals(object obj)
		{
			BaseCommand command = obj as BaseCommand;

			if (obj == null)
			{
				return false;
			}

			return base.Equals(obj) && CreationTime == command.CreationTime && Username  == command.Username;
		}

		/// <inheritdoc/>
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		/// <inheritdoc/>
		public override string ToString()
		{
			return $"{this.GetType().Name} : (id = {ID}) ";
		}
	}
}
