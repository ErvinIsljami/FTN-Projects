using System;
using System.Runtime.Serialization;

namespace Common.Commanding
{
	/// <summary>
	/// Command used by users to register to banking system.
	/// </summary>
	[DataContract]
	[Serializable]
	public class RegistrationCommand : BaseCommand
	{
		public RegistrationCommand()
		{

		}

		/// <summary>
		/// Initializes new instance of <see cref="RegistrationCommand"/> class. 
		/// </summary>
		/// <param name="commandId">Unique commanding ID.</param>
		/// <param name="username">New users username.</param>
		/// <param name="password">New users password.</param>
		public RegistrationCommand(long commandId, string username) : base(commandId, username)
		{
		}

		/// <inheritdoc/>
		public override bool Equals(object obj)
		{
			RegistrationCommand registraionCommand = obj as RegistrationCommand;
			if (registraionCommand == null)
			{
				return false;
			}

			return base.Equals(obj);
		}

		/// <inheritdoc/>
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public override string StringifyCommand()
		{
			return $"{Username} wants to register to the system."; 
		}
	}
}
