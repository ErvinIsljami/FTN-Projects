namespace Common.Model
{
	public class BankAccount : IdentifiedObject
	{
		public BankAccount() : base() { }
		public BankAccount(long accountNumber) : base(accountNumber)
		{
			AccountName = accountNumber;
		}

		public long AccountName { get; set; }
		public double Amount { get; set; }
		public string Username { get; set; }
		public long? UserId { get; set; }
		public User User { get; set; }

		public override string ToString()
		{
			return AccountName.ToString();
		}
	}
}
