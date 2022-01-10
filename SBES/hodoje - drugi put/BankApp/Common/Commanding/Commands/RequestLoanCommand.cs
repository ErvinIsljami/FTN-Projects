using System;
using System.Runtime.Serialization;

namespace Common.Commanding
{
	/// <summary>
	/// Command used to represent loan request initiated by user.
	/// </summary>
	[DataContract]
	[Serializable]
	public class RequestLoanCommand : BaseCommand
	{
		public RequestLoanCommand()
		{

		}

		/// <summary>
		/// Initializes new instance of <see cref="RequestLoanCommand"/> class.
		/// </summary>
		/// <param name="commandId">Unique command ID.</param>
		/// <param name="username">Username of user who requested loan.</param>
		/// <param name="amount">Loan amount.</param>
		/// <param name="months">Months to return the loan.</param>
		public RequestLoanCommand(long commandId, string username, double amount, int months) : base(commandId, username)
		{
			Amount = amount;
			Months = months;
		}

		/// <summary>
		/// Requested loan amount.
		/// </summary>
		[DataMember]
		public double Amount { get; private set; }

		/// <summary>
		/// Months to return the loan.
		/// </summary>
		[DataMember]
		public int Months { get; set; }

		/// <inheritdoc/>
		public override bool Equals(object obj)
		{
			RequestLoanCommand requestLoanCommand = obj as RequestLoanCommand;
			if (requestLoanCommand == null)
			{
				return false;
			}

			return base.Equals(obj) && Amount == requestLoanCommand.Amount;
		}

		/// <inheritdoc/>
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public override string StringifyCommand()
		{
			return $"{Username}: request a loan of {Amount}$ and return it in {Months} months.";
		}
	}
}
